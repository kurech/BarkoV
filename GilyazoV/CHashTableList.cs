using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilyazoV
{
    public class CHashTableList
    {
        private List<THashTable> arrFHashTableList = new List<THashTable>();
        private byte byteFTablesSize;

        public CHashTableList(byte byteATableCount)
        {
            this.byteFTablesSize = byteATableCount;
            for (int i = 0; i < byteATableCount; i++)
            {
                arrFHashTableList.Add(new THashTable());
            }
        }

        public bool SearchLexicalUnit(string strALexicalUnit, byte byteATable, ref int intALexicalCode)
        {
            return arrFHashTableList[byteATable].SearchLexicalUnit(strALexicalUnit, ref intALexicalCode);
        }

        public bool AddLexicalUnit(string strALexicalUnit, byte byteATable, ref int intALexicalCode)
        {
            return arrFHashTableList[byteATable].AddLexicalUnit(strALexicalUnit, ref intALexicalCode);
        }
        public bool ChangeLexicalUnit(string strALexicalUnit, byte byteATable, string newLexUnit)
        {
            int d = 0;
            arrFHashTableList[byteATable].DeleteLexicalUnit(strALexicalUnit);
            return arrFHashTableList[byteATable].AddLexicalUnit(newLexUnit, ref d);

        }
        public bool DeleteLexicalUnit(string strALexicalUnit, byte byteATable)
        {
            return arrFHashTableList[byteATable].DeleteLexicalUnit(strALexicalUnit);
        }
        public void TableToStringList(byte byteATable, List<string> sList)
        {
            arrFHashTableList[byteATable].GetLexicalUnitList(ref sList);
        }

        public int GetHashIndex(byte Table)
        {
            return arrFHashTableList[Table].intFHashIndex;
        }

    }
}
