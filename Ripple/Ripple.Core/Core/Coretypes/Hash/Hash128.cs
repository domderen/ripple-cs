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

        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.Hash128Field EmailHash = new Hash128Field(Field.EmailHash);

        public override object ToJson()
        {
            return InTranslate.ToJson(this);
        }

        public override byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public override string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public override void ToBytesSink(IBytesSink to)
        {
            InTranslate.ToBytesSink(this, to);
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

        public class OutTranslator : OutHashTranslator<Hash128>
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

        public class InTranslator : InHashTranslator<Hash128>
        {
        }
    }
}
