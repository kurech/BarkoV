using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilyazoV
{
    public class TableItem
    {
        public int value;
        public string lex;
        public TableItem next;
        public TableItem(int v, string s, TableItem t)
        {
            value = v;
            lex = s;
            next = t;
        }
    }

    public class THashTable
    {
        public List<TableItem> arrFHashTable = new List<TableItem>();
        public int intFHashIndex;
        static int tableSize = 20;

        public THashTable()
        {
            Init(tableSize);
        }
        public void Init(int count)
        {
            arrFHashTable.Clear();
            Resize(arrFHashTable, count);
        }

        static void Resize(List<TableItem> list, int size)
        {
            if (size > list.Count)
                while (size > list.Count)
                    list.Add(new TableItem(0, "", null));
            else if (size < list.Count)
                while (list.Count - size > 0)
                    list.RemoveAt(list.Count - 1);
        }

        int HashFunction(string strALexicalUnit)
        {
            int h = 0;
            for (int i = 0, l = strALexicalUnit.Length; i < l; i++)
            {
                h += strALexicalUnit[i];
            }
            return h % tableSize;
        }
        public void HashIndex(string strALexicalUnit)
        {
            int h = HashFunction(strALexicalUnit);
            intFHashIndex = h;
        }

        public bool SearchLexicalUnit(string strAlexicalUnit, ref int intALexicalCode)
        {
            HashIndex(strAlexicalUnit);
            if (arrFHashTable[intFHashIndex].lex == "") return false;
            else
            {
                intALexicalCode = arrFHashTable[intFHashIndex].value;
                return true;
            }
        }
        public bool ChangeLexicalUnit(string strPrevUnit, string strNewUnit)
        {
            HashIndex(strPrevUnit);
            TableItem item = arrFHashTable[intFHashIndex];

            while (item.next != null && item.lex != strPrevUnit)
            {
                item = item.next;
            }

            if (item.lex == strPrevUnit)
            {
                item.lex = strNewUnit;
            }
            return false;
        }
        public bool AddLexicalUnit(string strALexicalUnit, ref int intALexicalCode)
        {
            HashIndex(strALexicalUnit);
            intALexicalCode = intFHashIndex;
            TableItem item = arrFHashTable[intFHashIndex];
            TableItem prev = arrFHashTable[intFHashIndex];

            bool exist = false;
            if (prev.lex == strALexicalUnit)
            {
                exist = true;
            }

            while (prev.next != null)
            {
                prev = prev.next;
                if (prev.lex == strALexicalUnit)
                {
                    exist = true;
                }
            }

            if (!exist)
            {
                if (item.lex == prev.lex && item.lex == "")
                {
                    item.value = intALexicalCode;
                    item.lex = strALexicalUnit;
                }
                else
                {
                    TableItem newItem = new TableItem(intALexicalCode, strALexicalUnit, null);
                    prev.next = newItem;
                }

                //MessageBox.Show($"{item.lex}, {item.value}");
                return true;
            }
            return false;
        }
        public bool DeleteLexicalUnit(string strALexicalUnit)
        {
            HashIndex(strALexicalUnit);
            int indx = intFHashIndex;
            if (arrFHashTable[indx] != null)
            {
                TableItem item = arrFHashTable[indx];
                while (item.next != null && item.lex != strALexicalUnit)
                {
                    item = item.next;
                }

                if (item.lex == strALexicalUnit)
                {
                    if (item.next == null)
                    {
                        TableItem prev = arrFHashTable[indx];
                        while (prev.next != null && prev.next != item)
                        {
                            prev = prev.next;
                        }
                        prev.next = null;
                        item.value = 0;
                        item.lex = "";
                    }
                    else
                    {
                        TableItem prev = arrFHashTable[indx];
                        while (prev.next != null && prev.next != item)
                        {
                            prev = prev.next;
                        }
                        item.value = 0;
                        item.lex = "";
                        prev.next = item.next;

                    }
                    return true;
                }
            }
            return false;
        }

        public void GetLexicalUnitList(ref List<string> sList)
        {
            for (int i = 0; i < tableSize; i++)
            {
                TableItem item = arrFHashTable[i];
                while (item != null)
                {
                    if (item.lex != "")
                    {
                        sList.Add($"{item.lex}");
                    }
                    item = item.next;
                }
            }
        }
    }
}
