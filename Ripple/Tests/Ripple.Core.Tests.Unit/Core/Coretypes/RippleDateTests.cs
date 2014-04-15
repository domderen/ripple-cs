using NUnit.Framework;
using Ripple.Core.Core.Coretypes;

namespace Ripple.Core.Tests.Unit.Core.Coretypes
{
    [TestFixture]
    public class RippleDateTests
    {
        [Test]
        public void StaticConstructor_DoesNotThrowExceptions()
        {
            Assert.DoesNotThrow(() =>
            {
                var rippleDate = RippleDate.Now;
            });
        }

        [Test]
        public void TestDateParsing()
        {
            const int seconds = 442939950;
            RippleDate d = RippleDate.FromSecondsSinceRippleEpoch(seconds);
            var dateObj = d.Date;

            Assert.AreEqual(13, dateObj.Day);
            Assert.AreEqual(1, dateObj.Month);
            Assert.AreEqual(2014, dateObj.Year);

            Assert.AreEqual(14, dateObj.Hour);
            Assert.AreEqual(52, dateObj.Minute);
            Assert.AreEqual(30, dateObj.Second);

            Assert.AreEqual(seconds, d.SecondsSinceRippleEpoch());
        }
    }
}
