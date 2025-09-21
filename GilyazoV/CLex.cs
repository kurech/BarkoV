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
        lxmAnd, lxmLeftParenthSqr, lxmRightParenthSqr, lxmtz, lxmDD
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
                char[] reservedSymbols = new char[] { '(', ')', '[', ']', '.', ',', '!', '?', '$', '&', '-', '+', ';', ':', '/', '*' };

                chrFSelection = strFSource[intFSourceRowSelection][intFSourceColSelection]; //классификация прочитанной литеры
                if (chrFSelection == ' ') enumFSelectionCharType = TCharType.Space;
                else if (chrFSelection >= 'a' && chrFSelection <= 'd') enumFSelectionCharType = TCharType.Letter;
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
                        A:
                        {
                            if (chrFSelection == 'a')
                            {
                                TakeSymbol();
                                goto B;
                            }
                            else throw new Exception("Слово должно начинаться с 'ad'");
                        }
                        B:
                        {

                            if (chrFSelection == 'd')
                            {
                                TakeSymbol();
                                goto CFin;
                            }
                            else throw new Exception("Слово должно начинаться с 'ad'");
                        }
                        CFin:
                        {
                            if (chrFSelection == 'a' || chrFSelection == 'b' || chrFSelection == 'c' || chrFSelection == 'd')
                            {
                                TakeSymbol();
                                goto CFin;
                            }
                            else
                            {
                                enumFToken = TToken.lxmIdentifier;
                                return;
                            }
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
                            enumFToken = TToken.lxmDD;
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
