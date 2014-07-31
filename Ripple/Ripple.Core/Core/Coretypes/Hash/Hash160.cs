using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.Hash
{
    public class Hash160 : Hash<Hash160>
    {
        public Hash160(byte[] bytes)
            : base(bytes, 20)
        {
        }

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

        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.Hash160Field TakerPaysIssuer = new Hash160Field(Field.TakerPaysIssuer);
        public static TypedFields.Hash160Field TakerGetsCurrency = new Hash160Field(Field.TakerGetsCurrency);
        public static TypedFields.Hash160Field TakerPaysCurrency = new Hash160Field(Field.TakerPaysCurrency);
        public static TypedFields.Hash160Field TakerGetsIssuer = new Hash160Field(Field.TakerGetsIssuer);

        public class Hash160Field : TypedFields.Hash160Field
        {
            private readonly Field _f;

            public Hash160Field(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }

        public class OutTranslator : OutHashTranslator<Hash160>
        {
            public override Hash160 NewInstance(byte[] b)
            {
                return new Hash160(b);
            }

            public override int ByteWidth()
            {
                return 20;
            }

            public override Hash160 FromString(string s)
            {
                if (s.StartsWith("r"))
                {
                    return NewInstance(AccountId.FromAddress(s).Bytes);
                }

                return base.FromString(s);
            }
        }

        public class InTranslator : InHashTranslator<Hash160>
        {
        }
    }
}
