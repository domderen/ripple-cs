using System.Collections.Generic;
using Ripple.Core.Core.Enums;

namespace Ripple.Core.Core.Fields
{
    public static class FieldSymbolics
    {
        public static Dictionary<Field, IEnums> Lookup = new Dictionary<Field, IEnums>();

        static FieldSymbolics()
        {
            Lookup.Add(Field.TransactionType, new TransactionTypeEnums());
            Lookup.Add(Field.LedgerEntryType, new LedgerEntryTypeEnums());
            Lookup.Add(Field.TransactionResult, new TransactionEngineResultEnums());
        }

        public static bool IsSymbolicField(Field field)
        {
            return Lookup.ContainsKey(field);
        }

        public static string AsString(Field f, int i)
        {
            return Lookup[f].AsString(i);
        }

        public static int AsInteger(Field f, string s)
        {
            return Lookup[f].AsInteger(s);
        }

        private class TransactionEngineResultEnums : IEnums
        {
            public string AsString(int i)
            {
                return TransactionEngineResult.FromNumber(i).GetName();
            }

            public int AsInteger(string s)
            {
                return TransactionEngineResult.FromString(s).AsInteger;
            }
        }

        private class LedgerEntryTypeEnums : IEnums
        {
            public string AsString(int i)
            {
                return LedgerEntryType.FromNumber(i).GetName();
            }

            public int AsInteger(string s)
            {
                return LedgerEntryType.FromString(s).AsInteger;
            }
        }

        private class TransactionTypeEnums : IEnums
        {
            public string AsString(int i)
            {
                return TransactionType.FromNumber(i).GetName();
            }

            public int AsInteger(string s)
            {
                return TransactionType.FromString(s).AsInteger;
            }
        }
    }
}
