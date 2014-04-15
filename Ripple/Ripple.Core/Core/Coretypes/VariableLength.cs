using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Coretypes
{
    public class VariableLength : ISerializedType
    {
        public static Translator Translate = new Translator();
        public static TypedFields.VariableLengthField PublicKey = new VariableLengthField(Field.PublicKey);
        public static TypedFields.VariableLengthField MessageKey = new VariableLengthField(Field.MessageKey);
        public static TypedFields.VariableLengthField SigningPubKey = new VariableLengthField(Field.SigningPubKey);
        public static TypedFields.VariableLengthField TxnSignature = new VariableLengthField(Field.TxnSignature);
        public static TypedFields.VariableLengthField Generator = new VariableLengthField(Field.Generator);
        public static TypedFields.VariableLengthField Signature = new VariableLengthField(Field.Signature);
        public static TypedFields.VariableLengthField Domain = new VariableLengthField(Field.Domain);
        public static TypedFields.VariableLengthField FundCode = new VariableLengthField(Field.FundCode);
        public static TypedFields.VariableLengthField RemoveCode = new VariableLengthField(Field.RemoveCode);
        public static TypedFields.VariableLengthField ExpireCode = new VariableLengthField(Field.ExpireCode);
        public static TypedFields.VariableLengthField CreateCode = new VariableLengthField(Field.CreateCode);

        private readonly byte[] _buffer;

        public VariableLength(byte[] bytes)
        {
            _buffer = bytes;
        }

        public object ToJson()
        {
            return Translate.ToJson(this);
        }

        public byte[] ToBytes()
        {
            return Translate.ToBytes(this);
        }

        public string ToHex()
        {
            return Translate.ToHex(this);
        }

        public void ToBytesSink(IBytesSink to)
        {
            Translate.ToBytesSink(this, to);
        }

        public class Translator : TypeTranslator<VariableLength>
        {
            public override VariableLength FromParser(BinaryParser parser, int? hint)
            {
                if (hint == null)
                {
                    hint = parser.Size;
                }

                return new VariableLength(parser.Read((int)hint));
            }

            public override object ToJson(VariableLength obj)
            {
                return ToString(obj);
            }

            public override string ToString(VariableLength obj)
            {
                return B16.ToString(obj._buffer);
            }

            public override VariableLength FromString(string s)
            {
                return new VariableLength(Hex.Decode(s));
            }

            public override void ToBytesSink(VariableLength obj, IBytesSink to)
            {
                to.Add(obj._buffer);
            }
        }

        private class VariableLengthField : TypedFields.VariableLengthField
        {
            private readonly Field _f;

            public VariableLengthField(Field f)
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
