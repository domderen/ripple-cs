namespace Ripple.Core.Tests.Unit.Core.Coretypes.UInt
{
    using NUnit.Framework;

    using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;
    using UInt64 = Ripple.Core.Core.Coretypes.UInt.UInt64;

    [TestFixture]
    public class UIntTests
    {
        [Test]
        public void TestLte()
        {
            var n = new UInt64((long)34);
            var n2 = new UInt32((long)400);

            Assert.IsTrue(n.Lte(n2));
        }
    }
}
