using System;
using Deveel.Math;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public abstract class UIntBase : Number, ISerializedType, IComparable<UIntBase>
    {
        protected BigInteger _value;

        protected UIntBase()
        {
        }

        protected UIntBase(byte[] bytes)
        {
            SetValue(new BigInteger(1, bytes));
        }

        protected UIntBase(long s)
        {
            SetValue(BigInteger.ValueOf(s));
        }

        protected UIntBase(string s)
        {
            SetValue(new BigInteger(s));
        }

        protected UIntBase(BigInteger bi)
        {
            SetValue(bi);
        }

        protected UIntBase(string s, int radix)
        {
            SetValue(new BigInteger(s, radix));
        }

        public static BigInteger Max8 = new BigInteger("256");
        public static BigInteger Max16 = new BigInteger("65536");
        public static BigInteger Max32 = new BigInteger("4294967296");
        public static BigInteger Max64 = new BigInteger("18446744073709551616");

        public BigInteger GetMinimumValue
        {
            get { return BigInteger.Zero; }
        }

        public int BitLength
        {
            get { return _value.BitLength; }
        }

        public BigInteger BigInteger
        {
            get { return _value; }
        }

        public void SetValue(BigInteger value)
        {
            _value = value;
        }

        public abstract int GetByteWidth();

        public abstract UIntBase InstanceFrom(BigInteger n);

        public abstract object Value();

        public Boolean IsValid(BigInteger n)
        {
            return !((BitLength / 8) > GetByteWidth());
        }

        public UIntBase Add(UIntBase val)
        {
            return InstanceFrom(_value.Add(val._value));
        }

        public UIntBase Substract(UIntBase val)
        {
            return InstanceFrom(_value.Subtract(val._value));
        }

        public UIntBase Multiply(UIntBase val)
        {
            return InstanceFrom(_value.Multiply(val._value));
        }

        public UIntBase Divide(UIntBase val)
        {
            return InstanceFrom(_value.Divide(val._value));
        }

        public UIntBase Or(UIntBase val)
        {
            return InstanceFrom(_value.Or(val._value));
        }

        public UIntBase ShiftLeft(int n)
        {
            return InstanceFrom(_value.ShiftLeft(n));
        }

        public UIntBase ShiftRight(int n)
        {
            return InstanceFrom(_value.ShiftRight(n));
        }

        public BigInteger Min(BigInteger val)
        {
            return _value.Min(val);
        }

        public BigInteger Max(BigInteger val)
        {
            return _value.Max(val);
        }

        public string ToString(int radix)
        {
            return _value.ToString(radix);
        }

        public byte[] ToByteArray()
        {
            int length = GetByteWidth();
            byte[] bytes = _value.ToByteArray();

            if (bytes[0] == 0)
            {
                if (bytes.Length - 1 > length)
                {
                    throw new ArgumentException("Standard length exceeded for value.");
                }

                var tmp = new byte[length];
                Array.Copy(bytes, 1, tmp, tmp.Length - (bytes.Length - 1), bytes.Length - 1);
                return tmp;
            }
            else
            {
                if (bytes.Length == length)
                {
                    return bytes;
                }

                if (bytes.Length > length)
                {
                    throw new ArgumentException("Standard length exceeded for value.");
                }

                var tmp = new byte[length];
                Array.Copy(bytes, 0, tmp, tmp.Length - bytes.Length, bytes.Length);
                return tmp;
            }
        }

        public override int IntValue()
        {
            return _value.ToInt32();
        }

        public override long LongValue()
        {
            return _value.ToInt64();
        }

        public override double DoubleValue()
        {
            return Double.Parse(_value.ToString());
        }

        public override float FloatValue()
        {
            return float.Parse(_value.ToString());
        }

        public override byte ByteValue()
        {
            return (byte)IntValue();
        }

        public override short ShortValue()
        {
            return (short)IntValue();
        }

        public Boolean Lte<T>(T sequence) where T : UIntBase
        {
            return CompareTo(sequence) < 1;
        }

        public int CompareTo(UIntBase val)
        {
            return _value.CompareTo(val._value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public abstract object ToJson();
        public abstract byte[] ToBytes();
        public abstract string ToHex();
        public abstract void ToBytesSink(IBytesSink to);
    }
}
