using Deveel.Math;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public class UInt16 : UInt<UInt16>
    {
        public UInt16(byte[] bytes)
            : base(bytes)
        {
        }

        public UInt16(BigInteger value)
            : base(value)
        {
        }

        public UInt16(long s)
            : base(s)
        {
        }

        public UInt16(string s)
            : base(s)
        {
        }

        public UInt16(string s, int radix)
            : base(s, radix)
        {
        }

        public static TypeTranslator<UInt16> Translate = new UInt16Translator();
        public static TypedFields.UInt16Field LedgerEntryType = new UInt16Field(Field.LedgerEntryType);
        public static TypedFields.UInt16Field TransactionType = new UInt16Field(Field.TransactionType);


        public override int GetByteWidth()
        {
            return 2;
        }

        public override UIntBase InstanceFrom(BigInteger n)
        {
            return new UInt16(n);
        }

        public override object Value()
        {
            return IntValue();
        }

        public override object ToJson()
        {
            return Translate.ToJson(this);
        }

        public override byte[] ToBytes()
        {
            return Translate.ToBytes(this);
        }

        public override string ToHex()
        {
            return Translate.ToHex(this);
        }

        public override void ToBytesSink(IBytesSink to)
        {
            Translate.ToBytesSink(this, to);
        }

        private class UInt16Translator : UIntTranslator<UInt16>
        {
            public override UInt16 NewInstance(BigInteger i)
            {
                return new UInt16(i);
            }

            public override int ByteWidth()
            {
                return 2;
            }
        }

        private class UInt16Field : TypedFields.UInt16Field
        {
            private readonly Field _f;

            public UInt16Field(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }
    }
}
