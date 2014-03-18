namespace Ripple.Core.Core.Coretypes.UInt
{
    public abstract class Number
    {
        /// <summary>
        /// Returns the value of the specified number as an <see cref="int"/>. 
        /// This may involve rounding or truncation.
        /// </summary>
        /// <returns>
        /// the numeric value represented by this object after
        /// conversion to type <see cref="int"/>.
        /// </returns>
        public abstract int IntValue();

        /// <summary>
        /// Returns the value of the specified number as an <see cref="long"/>. 
        /// This may involve rounding or truncation.
        /// </summary>
        /// <returns>
        /// the numeric value represented by this object after
        /// conversion to type <see cref="long"/>.
        /// </returns>
        public abstract long LongValue();

        /// <summary>
        /// Returns the value of the specified number as an <see cref="float"/>. 
        /// This may involve rounding or truncation.
        /// </summary>
        /// <returns>
        /// the numeric value represented by this object after
        /// conversion to type <see cref="float"/>.
        /// </returns>
        public abstract float FloatValue();

        /// <summary>
        /// Returns the value of the specified number as an <see cref="double"/>. 
        /// This may involve rounding or truncation.
        /// </summary>
        /// <returns>
        /// the numeric value represented by this object after
        /// conversion to type <see cref="double"/>.
        /// </returns>
        public abstract double DoubleValue();

        /// <summary>
        /// Returns the value of the specified number as an <see cref="byte"/>. 
        /// This may involve rounding or truncation.
        /// </summary>
        /// <returns>
        /// the numeric value represented by this object after
        /// conversion to type <see cref="byte"/>.
        /// </returns>
        public virtual byte ByteValue()
        {
            return (byte)IntValue();
        }

        /// <summary>
        /// Returns the value of the specified number as an <see cref="short"/>. 
        /// This may involve rounding or truncation.
        /// </summary>
        /// <returns>
        /// the numeric value represented by this object after
        /// conversion to type <see cref="short"/>.
        /// </returns>
        public virtual short ShortValue()
        {
            return (short)IntValue();
        }
    }
}
