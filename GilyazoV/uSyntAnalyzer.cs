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

        public void S()
        {
            numTokenList.Clear();
            noDuplications = true;
            tree.Nodes.Clear();

            TreeNode parent = new TreeNode("S");
            tree.Nodes.Add(parent);

            if (Lex.enumPToken == TToken.lxmLeftParenth) //(A)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                A(parent);
                if (Lex.enumPToken == TToken.lxmRightParenth)
                {
                    AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
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
                else throw new Exception("Ожидалось ) [S]");
            }
            else throw new Exception("Ожидалось ( [S]");
        }
        public void A(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("A");
            highParent.Nodes.Add(parent);

            Lex.NextToken();
            if (Lex.enumPToken == TToken.lxmLeftParenth) //(<2>B)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                if (Lex.enumPToken == TToken.lxmIdentifier || Lex.enumPToken == TToken.lxmNumber)
                {
                    AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                    firstToken = (TToken)(1 - (int)Lex.enumPToken);
                    B(parent);
                    if (Lex.enumPToken == TToken.lxmRightParenth)
                    {
                        AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                        Lex.NextToken();
                    }
                    else throw new Exception("Ожидалось ) [A]");
                }
                else throw new Exception("Ожидался идентификатор или число [A]");
            }
            else //<1>
            {
                if (Lex.enumPToken == TToken.lxmIdentifier || Lex.enumPToken == TToken.lxmNumber)
                {
                    firstToken = Lex.enumPToken;
                    Lex.NextToken();
                }
                else throw new Exception("Ожидался идентификатор или число или ( [A]");
            }
        }
        public void B(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("B");
            highParent.Nodes.Add(parent);

            Lex.NextToken();
            if (Lex.enumPToken == TToken.lxmLeftParenth) //(C)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                C(parent);
                if (Lex.enumPToken == TToken.lxmRightParenth)
                {
                    AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                    Lex.NextToken();
                }
                else throw new Exception("Ожидалось ) [B]");
            }
            else throw new Exception("Ожидалось ( [B]");
        }
        public void C(TreeNode highParent)
        {
            TreeNode parent = new TreeNode("C");
            highParent.Nodes.Add(parent);

            if (Lex.enumPToken == firstToken)
            {
                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                D(parent);
            }
            else throw new Exception("Ожидался идентификатор или число [C]");
        }
        public void D(TreeNode highParent)
        {
            Lex.NextToken();
            if (Lex.enumPToken == TToken.lxmComma)
            {
                TreeNode parent = new TreeNode("D");
                highParent.Nodes.Add(parent);

                AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                Lex.NextToken();
                if (Lex.enumPToken == firstToken)
                {
                    AddTokenToTree(Lex.enumPToken, Lex.strPLexicalUnit, parent);
                    D(parent);
                }
                else throw new Exception("Ожидался идентификатор или число [D]");
            }
        }

    }
}
