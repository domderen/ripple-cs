using System;
using System.Globalization;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes
{
    public class RippleDate
    {
        public const long RippleEpochSecondsOffset = 0x386D4380;

        private DateTime _date;

        static RippleDate()
        {
            /**
             * Magic constant tested and documented.
             *
             * Seconds since the unix epoch from unix time (accounting leap years etc)
             * at 1/January/2000 GMT
             */

            var cal = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var computed =
                ((long)(cal.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds))/
                1000;
            AssertEquals("1/1/2000 12:00:00 AM", cal.ToString(CultureInfo.GetCultureInfo("En-us")));// TODO ?
            AssertEquals(RippleEpochSecondsOffset, computed);
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public static RippleDate Now
        {
            get
            {
                return new RippleDate();
            }
        }

        private RippleDate()
        {
            _date = DateTime.Now;
        }

        private RippleDate(long milliseconds)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var ts = new TimeSpan(dateTime.Ticks);

            long ms = ((long)ts.TotalMilliseconds) + milliseconds;

            _date = new DateTime(ms*10000);
        }

        public static RippleDate FromSecondsSinceRippleEpoch(long seconds)
        {
            return new RippleDate((seconds + RippleEpochSecondsOffset) * 1000);
        }

        public static RippleDate FromParser(BinaryParser parser)
        {
            var uint32 = UInt.UInt32.OutTranslate.FromParser(parser);
            return FromSecondsSinceRippleEpoch(uint32.LongValue());
        }

        private static void AssertEquals(string s, string s1)
        {
            if (s != s1)
            {
                throw new ApplicationException(string.Format("{0} != {1}", s, s1));
            }
        }

        private static void AssertEquals(long a, long b)
        {
            if (a != b)
            {
                throw new ApplicationException(string.Format("{0} != {1}", a, b));
            }
        }
        public long SecondsSinceRippleEpoch()
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var ticksSinceEpoch = _date.Ticks - dateTime.Ticks;

            return ((ticksSinceEpoch / 10000000) - RippleEpochSecondsOffset);
        }
    }
}
