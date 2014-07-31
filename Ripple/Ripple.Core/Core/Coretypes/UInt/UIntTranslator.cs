using Deveel.Math;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public abstract class OutUIntTranslator<T> : OutTypeTranslator<T> where T : UInt<T>
    {
        public abstract T NewInstance(BigInteger i);
        public abstract int ByteWidth();

        public override T FromParser(BinaryParser parser, int? hint)
        {
            return NewInstance(new BigInteger(1, parser.Read(ByteWidth())));
        }

        public override T FromLong(long aLong)
        {
            return NewInstance(BigInteger.ValueOf(aLong));
        }

        public override T FromString(string value)
        {
            int radix = this.ByteWidth() <= 4 ? 10 : 16;
            return NewInstance(new BigInteger(value, radix));
        }

        public override T FromInteger(int integer)
        {
            return FromLong((integer));
        }
    }

    public abstract class InUIntTranslator<T> : InTypeTranslator<T>
        where T : UInt<T>
    {
        public override object ToJson(T obj)
        {
            if (obj.GetByteWidth() <= 4)
            {
                return obj.IntValue();
            }

            return ToString(obj);
        }

        public override string ToString(T obj)
        {
            return B16.ToString(obj.ToByteArray());
        }

        public override void ToBytesSink(T obj, IBytesSink to)
        {
            to.Add(obj.ToByteArray());
        }
    }
}
