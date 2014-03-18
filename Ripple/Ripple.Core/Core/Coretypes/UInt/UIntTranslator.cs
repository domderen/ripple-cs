using Org.BouncyCastle.Math;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public abstract class UIntTranslator<T> : TypeTranslator<T> where T : UInt<T>
    {
        public abstract T NewInstance(BigInteger i);
        public abstract int ByteWidth();

        public override T FromParser(BinaryParser parser, int? hint)
        {
            return NewInstance(new BigInteger(1, parser.Read(ByteWidth())));
        }

        public new object ToJson(T obj)
        {
            if (obj.GetByteWidth() <= 4)
            {
                return obj.IntValue();
            }

            return ToString(obj);
        }

        public new T FromLong(long aLong)
        {
            return NewInstance(BigInteger.ValueOf(aLong));
        }

        public new T FromString(string value)
        {
            return NewInstance(new BigInteger(value, 16));
        }

        public new T FromInteger(int integer)
        {
            return FromLong((integer));
        }

        public new string ToString(T obj)
        {
            return B16.ToString(obj.ToByteArray());
        }

        public new void ToBytesSink(T obj, IBytesSink to)
        {
            to.Add(obj.ToByteArray());
        }
    }
}
