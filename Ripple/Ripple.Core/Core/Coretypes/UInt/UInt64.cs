using Deveel.Math;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public class UInt64 : UInt<UInt64>
    {
        public UInt64(byte[] bytes)
            : base(bytes)
        {
        }

        public UInt64(BigInteger value)
            : base(value)
        {
        }

        public UInt64(long s)
            : base(s)
        {
        }

        public UInt64(string s)
            : base(s)
        {
        }

        public UInt64(string s, int radix)
            : base(s, radix)
        {
        }

        public static TypeTranslator<UInt64> Translate = new UInt64Translator();
        public static TypedFields.UInt64Field IndexNext = new UInt64Field(Field.IndexNext);
        public static TypedFields.UInt64Field IndexPrevious = new UInt64Field(Field.IndexPrevious);
        public static TypedFields.UInt64Field BookNode = new UInt64Field(Field.BookNode);
        public static TypedFields.UInt64Field OwnerNode = new UInt64Field(Field.OwnerNode);
        public static TypedFields.UInt64Field BaseFee = new UInt64Field(Field.BaseFee);
        public static TypedFields.UInt64Field ExchangeRate = new UInt64Field(Field.ExchangeRate);
        public static TypedFields.UInt64Field LowNode = new UInt64Field(Field.LowNode);
        public static TypedFields.UInt64Field HighNode = new UInt64Field(Field.HighNode);

        public override int GetByteWidth()
        {
            return 8;
        }

        public override UIntBase InstanceFrom(BigInteger n)
        {
            return new UInt64(n);
        }

        public override object Value()
        {
            return BigInteger;
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

        private class UInt64Translator : UIntTranslator<UInt64>
        {
            public override UInt64 NewInstance(BigInteger i)
            {
                return new UInt64(i);
            }

            public override int ByteWidth()
            {
                return 8;
            }
        }

        private class UInt64Field : TypedFields.UInt64Field
        {
            private readonly Field _f;

            public UInt64Field(Field f)
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
