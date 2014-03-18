using System.Collections.Generic;

namespace Ripple.Core.Core.Fields
{
    /// <summary>
    /// In Java this type is auto-generated.
    /// </summary>
    public class Type
    {
        private static readonly Dictionary<int, Type> ByInt = new Dictionary<int, Type>();
        private readonly int _id;

        static Type()
        {
            foreach (var value in Values)
            {
                ByInt.Add(value.Id, value);
            }
        }

        private Type(int type)
        {
            _id = type;
        }

        public static readonly Type Unknown = new Type(-2);
        public static readonly Type Done = new Type(-1);
        public static readonly Type NotPresent = new Type(0);
        public static readonly Type UInt16 = new Type(1);
        public static readonly Type UInt32 = new Type(2);
        public static readonly Type UInt64 = new Type(3);
        public static readonly Type Hash128 = new Type(4);
        public static readonly Type Hash256 = new Type(5);
        public static readonly Type Amount = new Type(6);
        public static readonly Type VariableLength = new Type(7);
        public static readonly Type AccountId = new Type(8);
        public static readonly Type StObject = new Type(14);
        public static readonly Type StArray = new Type(15);
        public static readonly Type UInt8 = new Type(16);
        public static readonly Type Hash160 = new Type(17);
        public static readonly Type PathSet = new Type(18);
        public static readonly Type Vector256 = new Type(19);
        public static readonly Type Transaction = new Type(10001);
        public static readonly Type LedgerEntry = new Type(10002);
        public static readonly Type Validation = new Type(10003);

        public static IEnumerable<Type> Values
        {
            get
            {
                yield return Unknown;
                yield return Done;
                yield return NotPresent;
                yield return UInt16;
                yield return UInt32;
                yield return UInt64;
                yield return Hash128;
                yield return Hash256;
                yield return Amount;
                yield return VariableLength;
                yield return AccountId;
                yield return StObject;
                yield return StArray;
                yield return UInt8;
                yield return Hash160;
                yield return PathSet;
                yield return Vector256;
                yield return Transaction;
                yield return LedgerEntry;
                yield return Validation;
            }
        }

        public static Type ValueOf(int integer)
        {
            return ByInt[integer];
        }

        public int Id
        {
            get { return _id; }
        }
    }
}
