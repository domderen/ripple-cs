using System;
using Org.BouncyCastle.Math;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public abstract class UInt<TSubclass> : Number, ISerializedType, IComparable<UInt<TSubclass>> where TSubclass : UInt<TSubclass>
    {
        private BigInteger _value;

        protected UInt()
        {
        }

        protected UInt(byte[] bytes)
        {
            SetValue(new BigInteger(1, bytes));
        }

        protected UInt(Number s)
        {
            SetValue(BigInteger.ValueOf(s.LongValue()));
        }

        protected UInt(string s)
        {
            SetValue(new BigInteger(s));
        }

        protected UInt(BigInteger bi)
        {
            SetValue(bi);
        }

        protected UInt(string s, int radix)
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
        public abstract TSubclass InstanceFrom(BigInteger n);
        public abstract object Value();

        public Boolean IsValid(BigInteger n)
        {
            return !((BitLength / 8) > GetByteWidth());
        }

        public TSubclass Add(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Add(val._value));
        }

        public TSubclass Substract(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Subtract(val._value));
        }

        public TSubclass Multiply(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Multiply(val._value));
        }

        public TSubclass Divide(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Divide(val._value));
        }

        public TSubclass Or(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Or(val._value));
        }

        public TSubclass ShiftLeft(int n)
        {
            return InstanceFrom(_value.ShiftLeft(n));
        }

        public TSubclass ShiftRight(int n)
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
            return _value.IntValue;
        }

        public override long LongValue()
        {
            return _value.LongValue;
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
            return (byte) IntValue();
        }

        public override short ShortValue()
        {
            return (short) IntValue();
        }

        public Boolean Lte<T>(T sequence) where T : UInt<TSubclass>
        {
            return CompareTo(sequence) < 1;
        }

        public int CompareTo(UInt<TSubclass> val)
        {
            return _value.CompareTo(val._value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is UInt<TSubclass>)
            {
                return Equals((UInt<TSubclass>) obj);
            }

            return ReferenceEquals(this, obj);
        }

        public bool Equals(UInt<TSubclass> x)
        {
            return _value.Equals(x._value);
        }

        public abstract object ToJson();
        public abstract byte[] ToBytes();
        public abstract string ToHex();
        public abstract void ToBytesSink(IBytesSink to);
    }
}
