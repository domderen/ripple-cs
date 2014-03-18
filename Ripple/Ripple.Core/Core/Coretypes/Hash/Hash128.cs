using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.Hash
{
    public class Hash128 : Hash<Hash128>
    {
        public Hash128(byte[] bytes)
            : base(bytes, 16)
        {
        }

        public static Translator Translate = new Translator();
        public static TypedFields.Hash128Field EmailHash = new Hash128Field(Field.EmailHash);

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

        public class Hash128Field : TypedFields.Hash128Field
        {
            private readonly Field _f;

            public Hash128Field(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }

        public class Translator : HashTranslator<Hash128>
        {
            public override Hash128 NewInstance(byte[] b)
            {
                return new Hash128(b);
            }

            public override int ByteWidth()
            {
                return 16;
            }
        }
    }
}
