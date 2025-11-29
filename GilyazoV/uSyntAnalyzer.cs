using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilyazoV
{
    public class uSyntAnalyzer
    {
        private String[] strFSource;
        private String[] strFMessage;
        public String[] strPSource { set { strFSource = value; } get { return strFSource; } }
        public String[] strPMessage { set { strFMessage = value; } get { return strFMessage; } }
        public CLex Lex = new CLex();

        public TToken firstToken;

        public List<string> numTokenList = new List<string>();
        public bool noDuplications = true;

        public TreeView tree;

        public void AddTokenToTree(TToken token, string input, TreeNode parent)
        {
            if (token == TToken.lxmNumber)
            {
                for (int i = 0; i < numTokenList.Count; i++)
                {
                    if (input == numTokenList[i])
                    {
                        noDuplications = false;
                        //FindAndSelectNode(parent, "");
                    }
                }
                numTokenList.Add(input);
            }
            TreeNode newNode = new TreeNode(input);
            parent.Nodes.Add(newNode);
        }

        // S -> A; D
        public void S()
        {
            numTokenList.Clear();
            noDuplications = true;
            tree.Nodes.Clear();

            TreeNode parent = new TreeNode("S");
            tree.Nodes.Add(parent);

            A(parent);
            if (Lex.enumPToken == TToken.lxmtz) // ;
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                D(parent);
                tree.ExpandAll();
                if (!noDuplications)
                {
                    throw new Exception("Найдены повторяющиеся числа!");
                }
                else
                {
                    Semantic sem = new Semantic(tree);
                }
            }
            else throw new Exception("Ожидалось ; [S]");
        }

        // A -> int L
        public void A(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("A");
            highParent.Nodes.Add(parent);

            if (Lex.enumPToken == TToken.lxmInt)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                L(parent);
            }
            else throw new Exception("Ожидалось int [A]");
        }

        // L -> IX | I
        public void L(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("L");
            highParent.Nodes.Add(parent);

            I(parent);
            if (Lex.enumPToken == TToken.lxmComma) // X -> ,I | ,IX
            {
                X(parent);
            }
        }

        // X -> ,I | ,IX
        public void X(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("X");
            highParent.Nodes.Add(parent);

            if (Lex.enumPToken == TToken.lxmComma)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                I(parent);
                if (Lex.enumPToken == TToken.lxmComma) // Рекурсивный вызов для ,IX
                {
                    X(parent);
                }
            }
        }

        // I -> <2>:=<1>
        public void I(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("I");
            highParent.Nodes.Add(parent);

            if (Lex.enumPToken == TToken.lxmIdentifier) // <2>
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                if (Lex.enumPToken == TToken.lxmAssign) // :=
                {
                    AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                    Lex.NextToken();
                    if (Lex.enumPToken == TToken.lxmNumber) // <1>
                    {
                        AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                        Lex.NextToken();
                    }
                    else throw new Exception("Ожидалось число [I]");
                }
                else throw new Exception("Ожидалось := [I]");
            }
            else throw new Exception("Ожидался идентификатор [I]");
        }

        // D -> KY | K
        public void D(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("D");
            highParent.Nodes.Add(parent);

            K(parent);
            if (Lex.enumPToken == TToken.lxmOr) // Y -> or K | or KY
            {
                Y(parent);
            }
        }

        // Y -> or K | or KY
        public void Y(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("Y");
            highParent.Nodes.Add(parent);

            if (Lex.enumPToken == TToken.lxmOr)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                K(parent);
                if (Lex.enumPToken == TToken.lxmOr) // Рекурсивный вызов для or KY
                {
                    Y(parent);
                }
            }
        }

        // K -> RZ | R
        public void K(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("K");
            highParent.Nodes.Add(parent);

            R(parent);
            if (Lex.enumPToken == TToken.lxmAnd) // Z -> and R | and RY
            {
                Z(parent);
            }
        }

        // Z -> and R | and RY
        // RY означает R за которым следует Y
        public void Z(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("Z");
            highParent.Nodes.Add(parent);

            if (Lex.enumPToken == TToken.lxmAnd)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                R(parent);
                // Проверяем, есть ли Y (or K | or KY) после R
                if (Lex.enumPToken == TToken.lxmOr) // and RY - если после R идет or
                {
                    Y(parent);
                }
                // Иначе это просто "and R"
            }
        }

        // R -> V<V | V>V
        public void R(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("R");
            highParent.Nodes.Add(parent);

            V(parent);
            if (Lex.enumPToken == TToken.lxmLess) // V<V
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                V(parent);
            }
            else if (Lex.enumPToken == TToken.lxmGreater) // V>V
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                V(parent);
            }
            else throw new Exception("Ожидалось < или > [R]");
        }

        // V -> <2> | <1>
        public void V(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("V");
            highParent.Nodes.Add(parent);

            if (Lex.enumPToken == TToken.lxmIdentifier) // <2>
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
            }
            else if (Lex.enumPToken == TToken.lxmNumber) // <1>
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
            }
            else throw new Exception("Ожидался идентификатор или число [V]");
        }

    }
}
