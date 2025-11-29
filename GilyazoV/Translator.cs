using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GilyazoV
{
    public class Translator
    {
        private List<string> intermediateCode = new List<string>();
        private Dictionary<string, int> variables = new Dictionary<string, int>();
        private int tempCounter = 0;
        private int labelCounter = 0;
        private List<string> commandSequence = new List<string>();
        private int result = 0;

        public string GetIntermediateCode()
        {
            return string.Join("\r\n", intermediateCode);
        }

        public string GetCommandSequence()
        {
            return string.Join(", ", commandSequence);
        }

        public int GetResult()
        {
            return result;
        }

        // Преобразование двоичного числа в десятичное
        private int BinaryToDecimal(string binary)
        {
            int result = 0;
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1')
                {
                    result += (int)Math.Pow(2, binary.Length - 1 - i);
                }
            }
            return result;
        }

        // Генерация временной переменной
        private string GetTempVar()
        {
            return $"T{tempCounter++}";
        }

        // Генерация метки
        private string GetLabel()
        {
            return $"L{labelCounter++}";
        }

        // Перевод дерева на промежуточный язык
        public void Translate(TreeView tree)
        {
            intermediateCode.Clear();
            variables.Clear();
            tempCounter = 0;
            labelCounter = 0;
            commandSequence.Clear();

            foreach (TreeNode node in tree.Nodes)
            {
                if (node.Text == "S")
                {
                    TranslateS(node);
                }
            }
        }

        // S -> A; D
        private void TranslateS(TreeNode node)
        {
            // Сначала обрабатываем объявления (A) - ВАЖНО: сначала все объявления!
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "A")
                {
                    TranslateA(child);
                }
            }
            
            // После объявлений обрабатываем выражения (D) - ВАЖНО: после всех объявлений!
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "D")
                {
                    result = TranslateD(child);
                    // MEM добавляется только при операциях с памятью (MOV), не здесь
                }
            }
        }

        // A -> int L
        private void TranslateA(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "L")
                {
                    TranslateL(child);
                }
            }
        }

        // L -> IX | I
        private void TranslateL(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "I")
                {
                    TranslateI(child);
                }
                else if (child.Text == "X")
                {
                    TranslateX(child);
                }
            }
        }

        // X -> ,I | ,IX
        private void TranslateX(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "I")
                {
                    TranslateI(child);
                }
                else if (child.Text == "X")
                {
                    TranslateX(child);
                }
            }
        }

        // I -> <2>:=<1>
        private void TranslateI(TreeNode node)
        {
            string varName = "";
            int value = 0;

            // Ищем идентификатор и число в узлах
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text != ":=")
                {
                    // Проверяем, является ли это числом
                    bool isNumber = true;
                    foreach (char c in child.Text)
                    {
                        if (c != '0' && c != '1')
                        {
                            isNumber = false;
                            break;
                        }
                    }
                    
                    if (isNumber)
                    {
                        value = BinaryToDecimal(child.Text);
                    }
                    else
                    {
                        varName = child.Text;
                    }
                }
            }

            // MOV varName, value
            intermediateCode.Add($"MOV {varName}, {value}");
            commandSequence.Add("MEM");
            variables[varName] = value;
            // Отладка: проверяем, что переменная сохранена
            // System.Diagnostics.Debug.WriteLine($"Переменная {varName} = {value} сохранена в словарь");
        }

        // D -> KY | K
        private int TranslateD(TreeNode node)
        {
            int leftResult = 0;
            bool hasOr = false;
            bool foundK = false;

            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "K")
                {
                    leftResult = TranslateK(child);
                    foundK = true;
                    if (!hasOr)
                    {
                        result = leftResult;
                    }
                }
                else if (child.Text == "Y")
                {
                    hasOr = true;
                    int rightResult = TranslateY(child);
                    // OR операция
                    string tempVar = GetTempVar();
                    intermediateCode.Add($"OR {tempVar}, {leftResult}, {rightResult}");
                    commandSequence.Add("OR");
                    result = leftResult | rightResult; // Логическое ИЛИ
                    leftResult = result;
                }
            }

            // Если нашли только K без Y, возвращаем результат K
            if (foundK && !hasOr)
            {
                return leftResult;
            }

            return result;
        }

        // Y -> or K | or KY
        private int TranslateY(TreeNode node)
        {
            int result = 0;
            bool foundK = false;

            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "K")
                {
                    if (!foundK)
                    {
                        result = TranslateK(child);
                        foundK = true;
                    }
                    else
                    {
                        int rightResult = TranslateK(child);
                        string tempVar = GetTempVar();
                        intermediateCode.Add($"OR {tempVar}, {result}, {rightResult}");
                        commandSequence.Add("OR");
                        result = result | rightResult;
                    }
                }
                else if (child.Text == "Y")
                {
                    int rightResult = TranslateY(child);
                    string tempVar = GetTempVar();
                    intermediateCode.Add($"OR {tempVar}, {result}, {rightResult}");
                    commandSequence.Add("OR");
                    result = result | rightResult;
                }
            }

            return result;
        }

        // K -> RZ | R
        private int TranslateK(TreeNode node)
        {
            int leftResult = 0;
            bool hasAnd = false;
            bool foundR = false;

            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "R")
                {
                    leftResult = TranslateR(child);
                    foundR = true;
                    if (!hasAnd)
                    {
                        result = leftResult;
                    }
                }
                else if (child.Text == "Z")
                {
                    hasAnd = true;
                    int rightResult = TranslateZ(child);
                    // AND операция
                    string tempVar = GetTempVar();
                    intermediateCode.Add($"AND {tempVar}, {leftResult}, {rightResult}");
                    commandSequence.Add("AND");
                    result = leftResult & rightResult; // Логическое И
                    leftResult = result;
                }
            }

            // Если нашли только R без Z, возвращаем результат R
            if (foundR && !hasAnd)
            {
                return leftResult;
            }

            return result;
        }

        // Z -> and R | and RY
        private int TranslateZ(TreeNode node)
        {
            int result = 0;
            bool foundR = false;

            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "R")
                {
                    if (!foundR)
                    {
                        result = TranslateR(child);
                        foundR = true;
                    }
                    else
                    {
                        int rightResult = TranslateR(child);
                        string tempVar = GetTempVar();
                        intermediateCode.Add($"AND {tempVar}, {result}, {rightResult}");
                        commandSequence.Add("AND");
                        result = result & rightResult;
                    }
                }
                else if (child.Text == "Y")
                {
                    int rightResult = TranslateY(child);
                    string tempVar = GetTempVar();
                    intermediateCode.Add($"OR {tempVar}, {result}, {rightResult}");
                    commandSequence.Add("OR");
                    result = result | rightResult;
                }
            }

            return result;
        }

        // R -> V<V | V>V
        private int TranslateR(TreeNode node)
        {
            int leftValue = 0;
            int rightValue = 0;
            string leftOperand = "";
            string rightOperand = "";
            string operation = "";

            // Обрабатываем узлы по порядку: V, операция, V
            List<TreeNode> vNodes = new List<TreeNode>();
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == "V")
                {
                    vNodes.Add(child);
                }
                else if (child.Text == "<")
                {
                    operation = "<";
                }
                else if (child.Text == ">")
                {
                    operation = ">";
                }
            }

            if (vNodes.Count >= 2)
            {
                // Получаем значения и операнды (имена переменных или числа)
                var leftResult = TranslateVWithName(vNodes[0]);
                leftValue = leftResult.Value;
                leftOperand = leftResult.Operand;
                
                var rightResult = TranslateVWithName(vNodes[1]);
                rightValue = rightResult.Value;
                rightOperand = rightResult.Operand;
            }

            int comparisonResult = 0;
            string tempVar = GetTempVar();

            if (operation == "<")
            {
                // Сравнение: leftValue < rightValue
                string labelTrue = GetLabel();
                string labelEnd = GetLabel();
                // Используем имена переменных в коде, а не только значения
                intermediateCode.Add($"SUB {tempVar}, {rightOperand}, {leftOperand}");
                intermediateCode.Add($"JP {labelTrue}");
                intermediateCode.Add($"MOV {tempVar}, 0");
                intermediateCode.Add($"JMP {labelEnd}");
                intermediateCode.Add($"{labelTrue}:");
                intermediateCode.Add($"MOV {tempVar}, 1");
                intermediateCode.Add($"{labelEnd}:");
                commandSequence.Add("MEM");
                comparisonResult = (leftValue < rightValue) ? 1 : 0;
            }
            else if (operation == ">")
            {
                // Сравнение: leftValue > rightValue
                string labelTrue = GetLabel();
                string labelEnd = GetLabel();
                // Используем имена переменных в коде, а не только значения
                intermediateCode.Add($"SUB {tempVar}, {leftOperand}, {rightOperand}");
                intermediateCode.Add($"JP {labelTrue}");
                intermediateCode.Add($"MOV {tempVar}, 0");
                intermediateCode.Add($"JMP {labelEnd}");
                intermediateCode.Add($"{labelTrue}:");
                intermediateCode.Add($"MOV {tempVar}, 1");
                intermediateCode.Add($"{labelEnd}:");
                commandSequence.Add("MEM");
                comparisonResult = (leftValue > rightValue) ? 1 : 0;
            }

            return comparisonResult;
        }
        
        // Вспомогательная структура для возврата значения и операнда
        private struct ValueOperand
        {
            public int Value;
            public string Operand;
        }
        
        // V -> <2> | <1> (с возвратом имени операнда)
        private ValueOperand TranslateVWithName(TreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                string value = node.Nodes[0].Text;

                // Проверяем, является ли это числом
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
                    // Это число - преобразуем в десятичное
                    int numValue = BinaryToDecimal(value);
                    return new ValueOperand { Value = numValue, Operand = numValue.ToString() };
                }
                else
                {
                    // Это идентификатор - получаем значение из словаря переменных
                    if (variables.ContainsKey(value))
                    {
                        int varValue = variables[value];
                        // Используем имя переменной в коде
                        return new ValueOperand { Value = varValue, Operand = value };
                    }
                    else
                    {
                        // Если переменная не найдена, возвращаем 0, но используем имя переменной в коде
                        return new ValueOperand { Value = 0, Operand = value };
                    }
                }
            }

            return new ValueOperand { Value = 0, Operand = "0" };
        }

        // V -> <2> | <1>
        private int TranslateV(TreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                string value = node.Nodes[0].Text;

                // Проверяем, является ли это числом
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
                    // Это число - преобразуем в десятичное
                    return BinaryToDecimal(value);
                }
                else
                {
                    // Это идентификатор - получаем значение из словаря переменных
                    if (variables.ContainsKey(value))
                    {
                        int varValue = variables[value];
                        return varValue;
                    }
                    else
                    {
                        // Если переменная не найдена, возвращаем 0 (по умолчанию)
                        // Это позволяет использовать переменные без объявления
                        return 0;
                    }
                }
            }

            return 0;
        }
    }
}

