using System;
using Deveel.Math;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public abstract class UInt<TSubclass> : UIntBase, IComparable<UInt<TSubclass>> where TSubclass : UIntBase
    {
        protected UInt()
        {
        }

        protected UInt(byte[] bytes)
            : base(bytes)
        {
        }

        protected UInt(long s)
            : base(s)
        {
        }

        protected UInt(string s)
            : base(s)
        {
        }

        protected UInt(BigInteger bi)
            : base(bi)
        {
        }

        protected UInt(string s, int radix)
            : base(s, radix)
        {
        }

        public TSubclass Add(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Add(val._value)) as TSubclass;
        }

        public TSubclass Substract(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Subtract(val._value)) as TSubclass;
        }

        public TSubclass Multiply(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Multiply(val._value)) as TSubclass;
        }

        public TSubclass Divide(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Divide(val._value)) as TSubclass;
        }

        public TSubclass Or(UInt<TSubclass> val)
        {
            return InstanceFrom(_value.Or(val._value)) as TSubclass;
        }

        public new TSubclass ShiftLeft(int n)
        {
            return InstanceFrom(_value.ShiftLeft(n)) as TSubclass;
        }

        public new TSubclass ShiftRight(int n)
        {
            return InstanceFrom(_value.ShiftRight(n)) as TSubclass;
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
    }
}
