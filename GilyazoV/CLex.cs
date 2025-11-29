using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilyazoV
{
    public enum TState { Start, Continue, Finish }; //тип состояния
    public enum TCharType { Letter, Digit, EndRow, EndText, Space, ReservedSymbol }; // тип символа
    public enum TToken
    {
        lxmIdentifier, lxmNumber, lxmUnknown, lxmEmpty, lxmLeftParenth, lxmRightParenth, lxmIs, lxmDot, lxmComma, lxmDollar, lxmMinus, lxmPlus, lxmExclamation, lxmQuestion,
        lxmAnd, lxmLeftParenthSqr, lxmRightParenthSqr, lxmtz, lxmDD, lxmInt, lxmOr, lxmAssign, lxmLess, lxmGreater
    };

    public class CLex //класс лексический анализатор
    {
        private String[] strFSource;  // указатель на массив строк
        private String[] strFMessage;  // указатель на массив строк
        public TCharType enumFSelectionCharType;
        public char chrFSelection;
        private TState enumFState;
        private int intFSourceRowSelection;
        private int intFSourceColSelection;
        private String strFLexicalUnit;
        private TToken enumFToken;
        public String[] strPSource { set { strFSource = value; } get { return strFSource; } }
        public String[] strPMessage { set { strFMessage = value; } get { return strFMessage; } }
        public TState enumPState { set { enumFState = value; } get { return enumFState; } }
        public String strPLexicalUnit { set { strFLexicalUnit = value; } get { return strFLexicalUnit; } }
        public TToken enumPToken { set { enumFToken = value; } get { return enumFToken; } }
        public int intPSourceRowSelection { get { return intFSourceRowSelection; } set { intFSourceRowSelection = value; } }
        public int intPSourceColSelection { get { return intFSourceColSelection; } set { intFSourceColSelection = value; } }

        public CLex()
        {
        }

        public void GetSymbol() //метод класса лексический анализатор
        {
            intFSourceColSelection++; // продвигаем номер колонки
            if (intFSourceColSelection > strFSource[intFSourceRowSelection].Length - 1)
            {
                intFSourceRowSelection++;
                if (intFSourceRowSelection <= strFSource.Length - 1)
                {
                    intFSourceColSelection = -1;
                    chrFSelection = '\0';
                    enumFSelectionCharType = TCharType.EndRow;
                    enumFState = TState.Continue;
                }
                else
                {
                    chrFSelection = '\0';
                    enumFSelectionCharType = TCharType.EndText;
                    enumFState = TState.Finish;

                }
            }
            else
            {
                char[] reservedSymbols = new char[] { '(', ')', '[', ']', '.', ',', '!', '?', '$', '&', '-', '+', ';', ':', '=', '*', '<', '>' };

                chrFSelection = strFSource[intFSourceRowSelection][intFSourceColSelection]; //классификация прочитанной литеры
                if (chrFSelection == ' ') enumFSelectionCharType = TCharType.Space;
                else if (chrFSelection >= 'a' && chrFSelection <= 'z') enumFSelectionCharType = TCharType.Letter;
                else if (chrFSelection == '0' || chrFSelection == '1') enumFSelectionCharType = TCharType.Digit;

                else if (reservedSymbols.Contains(chrFSelection)) enumFSelectionCharType = TCharType.ReservedSymbol;
                else throw new System.Exception("Cимвол вне алфавита");
                enumFState = TState.Continue;
            }
        }

        private void TakeSymbol()
        {
            char[] c = { chrFSelection };
            String s = new string(c);
            strPLexicalUnit += s;
            GetSymbol();
        }
        public void NextToken()
        {
            strPLexicalUnit = "";
            if (enumFState == TState.Start)
            {
                intFSourceRowSelection = 0;
                intFSourceColSelection = -1;
                GetSymbol();
            }

            while (enumFSelectionCharType == TCharType.Space || enumFSelectionCharType == TCharType.EndRow)
            {
                GetSymbol();
            }

            if (chrFSelection == '/')
            {
                GetSymbol();
                if (chrFSelection == '/')
                    while (enumFSelectionCharType != TCharType.EndRow)
                    {
                        GetSymbol();
                    }
                GetSymbol();
            }

            switch (enumFSelectionCharType)
            {
                case TCharType.Letter:
                    {
                        // Проверка на ключевое слово "int"
                        if (chrFSelection == 'i')
                        {
                            TakeSymbol();
                            if (chrFSelection == 'n')
                            {
                                TakeSymbol();
                                if (chrFSelection == 't')
                                {
                                    TakeSymbol();
                                    // Проверяем, что после "int" нет буквы (чтобы не было "inta")
                                    if (enumFSelectionCharType == TCharType.Letter)
                                    {
                                        throw new Exception("После ключевого слова 'int' не может быть буквы");
                                    }
                                    else
                                    {
                                        enumFToken = TToken.lxmInt;
                                        return;
                                    }
                                }
                                else
                                {
                                    throw new Exception("Идентификатор должен начинаться с 'ad'");
                                }
                            }
                            else
                            {
                                throw new Exception("Идентификатор должен начинаться с 'ad'");
                            }
                        }
                        // Проверка на ключевое слово "or"
                        else if (chrFSelection == 'o')
                        {
                            TakeSymbol();
                            if (chrFSelection == 'r')
                            {
                                TakeSymbol();
                                // Проверяем, что после "or" нет буквы
                                if (enumFSelectionCharType == TCharType.Letter)
                                {
                                    throw new Exception("После ключевого слова 'or' не может быть буквы");
                                }
                                else
                                {
                                    enumFToken = TToken.lxmOr;
                                    return;
                                }
                            }
                            else
                            {
                                throw new Exception("Идентификатор должен начинаться с 'ad'");
                            }
                        }
                        // Проверка на ключевое слово "and" или идентификатор, начинающийся с "ad"
                        else if (chrFSelection == 'a')
                        {
                            TakeSymbol();
                            if (chrFSelection == 'n')
                            {
                                TakeSymbol();
                                if (chrFSelection == 'd')
                                {
                                    TakeSymbol();
                                    // Проверяем, что после "and" нет буквы
                                    if (enumFSelectionCharType == TCharType.Letter)
                                    {
                                        throw new Exception("После ключевого слова 'and' не может быть буквы");
                                    }
                                    else
                                    {
                                        enumFToken = TToken.lxmAnd;
                                        return;
                                    }
                                }
                                else
                                {
                                    throw new Exception("Идентификатор должен начинаться с 'ad'");
                                }
                            }
                            else if (chrFSelection == 'd')
                            {
                                // Это идентификатор, начинающийся с "ad"
                                TakeSymbol();
                                // Продолжаем читать остальные буквы идентификатора
                                while (enumFSelectionCharType == TCharType.Letter)
                                {
                                    TakeSymbol();
                                }
                                enumFToken = TToken.lxmIdentifier;
                                return;
                            }
                            else
                            {
                                throw new Exception("Идентификатор должен начинаться с 'ad'");
                            }
                        }
                        // Любая другая буква - ошибка, так как идентификаторы должны начинаться с "ad"
                        else
                        {
                            throw new Exception("Идентификатор должен начинаться с 'ad'");
                        }
                    }
                    if (chrFSelection == '/')
                    {
                        GetSymbol();
                        if (chrFSelection == '/')
                            while (enumFSelectionCharType != TCharType.EndRow)
                            {
                                GetSymbol();
                            }
                        GetSymbol();
                    }
                case TCharType.Digit:
                    {
                        A:
                        {
                            if (chrFSelection == '0')
                            {
                                TakeSymbol();
                                goto B;
                            }
                            else if (chrFSelection == '1')
                            {
                                TakeSymbol();
                                goto D;
                            }
                            else throw new Exception("Ожидался 0 или 1");
                        }
                        B:
                        {
                            if (chrFSelection == '0')
                            {
                                TakeSymbol();
                                goto C;
                            }
                            else throw new Exception("Ожидался 0");
                        }
                        C:
                        {
                            if (chrFSelection == '0')
                            {
                                TakeSymbol();
                                goto A;
                            }
                            else throw new Exception("Ожидался 0");
                        }
                        D:
                        {
                            if (chrFSelection == '0')
                            {
                                TakeSymbol();
                                goto E;
                            }
                            else throw new Exception("Ожидался 0");
                        }
                        E:
                        {
                            if (chrFSelection == '1')
                            {
                                TakeSymbol();
                                goto FFin;
                            }
                            else throw new Exception("Ожидался 1");
                        }
                        FFin:
                        {
                            if (chrFSelection == '1')
                            {
                                TakeSymbol();
                                goto H;
                            }
                            else if (enumFSelectionCharType != TCharType.Digit)
                            {
                                enumFToken = TToken.lxmNumber;
                                return;
                            }
                            else throw new Exception("Ожидался 1");
                        }
                        G:
                        {
                            if (chrFSelection == '0')
                            {
                                TakeSymbol();
                                goto FFin;
                            }
                            else throw new Exception("Ожидалась 0");
                        }
                        H:
                        {
                            if (chrFSelection == '1')
                            {
                                TakeSymbol();
                                goto G;
                            }
                            else throw new Exception("Ожидался 1");
                        }
                    }
                case TCharType.ReservedSymbol:
                    {
                        if (chrFSelection == '/')
                        {
                            TakeSymbol();
                            if (chrFSelection == '/')
                            {
                                while (enumFSelectionCharType != TCharType.EndRow)
                                    TakeSymbol();
                            }
                            TakeSymbol();
                        }
                        if (chrFSelection == '(')
                        {
                            enumFToken = TToken.lxmLeftParenth;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == ')')
                        {
                            enumFToken = TToken.lxmRightParenth;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '.')
                        {
                            enumFToken = TToken.lxmDot;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == ',')
                        {
                            enumFToken = TToken.lxmComma;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '[')
                        {
                            enumFToken = TToken.lxmLeftParenthSqr;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == ']')
                        {
                            enumFToken = TToken.lxmRightParenthSqr;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '!')
                        {
                            enumFToken = TToken.lxmExclamation;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '?')
                        {
                            enumFToken = TToken.lxmQuestion;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '&')
                        {
                            enumFToken = TToken.lxmAnd;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '$')
                        {
                            enumFToken = TToken.lxmDollar;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '+')
                        {
                            enumFToken = TToken.lxmPlus;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '-')
                        {
                            enumFToken = TToken.lxmMinus;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == ';')
                        {
                            enumFToken = TToken.lxmtz;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == ':')
                        {
                            TakeSymbol();
                            if (chrFSelection == '=')
                            {
                                TakeSymbol();
                                enumFToken = TToken.lxmAssign;
                                return;
                            }
                            else
                            {
                                enumFToken = TToken.lxmDD;
                                return;
                            }
                        }
                        if (chrFSelection == '<')
                        {
                            enumFToken = TToken.lxmLess;
                            TakeSymbol();
                            return;
                        }
                        if (chrFSelection == '>')
                        {
                            enumFToken = TToken.lxmGreater;
                            TakeSymbol();
                            return;
                        }
                        break;
                    }

                case TCharType.EndText:
                    {
                        enumFToken = TToken.lxmEmpty;
                        break;
                    }
            }
        }
    }
}
