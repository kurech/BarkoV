using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilyazoV
{
    public class Semantic
    {
        public int countLetters = 0;
        public int countDigits = 0;
        public string strIndentifier;
        public string strDigital;
        private TreeView tree;

        public bool contains100 = false;

        public Semantic()
        {

        }
        public Semantic(TreeView treeView)
        {
            tree = treeView;
            TreeController(tree);
        }
        public void TreeController(TreeView tree)
        {
            foreach (TreeNode node in tree.Nodes)
            {
                TreeController(node);
            }
            Check();
        }

        public void TreeController(TreeNode node)
        {
            // I -> <2>:=<1> - идентификатор в первом узле, число в третьем узле
            if (node.Text == "I")
            {
                // Первый узел - идентификатор (<2>)
                if (node.Nodes.Count > 0)
                {
                    strIndentifier = node.Nodes[0].Text.ToString();
                    CheckLetters(strIndentifier);
                }
                // Третий узел - число (<1>)
                if (node.Nodes.Count > 2)
                {
                    strDigital = node.Nodes[2].Text.ToString();
                    isNumber100(strDigital);
                    countDigits++;
                }
            }
            // V -> <2> | <1> - может содержать идентификатор или число
            if (node.Text == "V")
            {
                if (node.Nodes.Count > 0)
                {
                    string value = node.Nodes[0].Text.ToString();
                    // Проверяем, является ли это числом (содержит только 0 и 1)
                    bool isNumber = true;
                    foreach (char c in value)
                    {
                        if (c != '0' && c != '1')
                        {
                            isNumber = false;
                            break;
                        }
                    }
                    if (isNumber)
                    {
                        strDigital = value;
                        isNumber100(strDigital);
                        countDigits++;
                    }
                    // Иначе это идентификатор
                    else
                    {
                        strIndentifier = value;
                        CheckLetters(strIndentifier);
                    }
                }
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                TreeController(childNode);
            }
        }

        //Проверка на наличие элемента "100", в случае равенства количества согласных в конце идентификатора с количеством блоков двоичной записи в конструкциях вида <2>, <2>, ... 
        private void Check()
        {
            if (countDigits != countLetters)
            {
                throw new Exception("Конец слова, текст верный.");
            }
            else
            {
                if (contains100)
                {
                    throw new Exception("Конец слова, текст верный.");
                }
                else
                {
                    throw new Exception("Число согласных соответствует числу элементов, но среди элементов нет 100");
                }
            }
        }

        private void isNumber100(string input)
        {
            if (input == "100")
            {
                contains100 = true;
            }
        }

        private void CheckLetters(string input)
        {
            for (int i = input.Length - 1; i >= 0; --i)
            {
                //Подсчет числа согласных
                if (input[i] == 'd' || input[i] == 'c' || input[i] == 'b')
                {
                    countLetters++;
                }
            }
        }
    }
}
