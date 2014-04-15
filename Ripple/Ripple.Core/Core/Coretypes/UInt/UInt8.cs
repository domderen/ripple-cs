
using Deveel.Math;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public class UInt8 : UInt<UInt8>
    {
        public UInt8(byte[] bytes)
            : base(bytes)
        {
        }

        public UInt8(BigInteger value)
            : base(value)
        {
        }

        public UInt8(long s)
            : base(s)
        {
        }

        public UInt8(string s)
            : base(s)
        {
        }

        public UInt8(string s, int radix)
            : base(s, radix)
        {
        }

        public static TypeTranslator<UInt8> Translate = new UInt8Translator();
        public static TypedFields.UInt8Field CloseResolution = new UInt8Field(Field.CloseResolution);
        public static TypedFields.UInt8Field TemplateEntryType = new UInt8Field(Field.TemplateEntryType);
        public static TypedFields.UInt8Field TransactionResult = new UInt8Field(Field.TransactionResult);

        public override int GetByteWidth()
        {
            return 1;
        }

        public override UIntBase InstanceFrom(BigInteger n)
        {
            return new UInt8(n);
        }

        public override object Value()
        {
            return base.ShortValue();
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

        private class UInt8Translator : UIntTranslator<UInt8>
        {
            public override UInt8 NewInstance(BigInteger i)
            {
                return new UInt8(i);
            }

            public override int ByteWidth()
            {
                return 1;
            }
        }

        private class UInt8Field : TypedFields.UInt8Field
        {
            private readonly Field _f;

            public UInt8Field(Field f)
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
