namespace Ripple.Core.Tests.Unit.Core.Formats
{
    using NUnit.Framework;

    using Ripple.Core.Core.Formats;

    [TestFixture]
    public class SleFormatTest
    {
        [Test]
        public void TestFromValue()
        {
            SleFormat accountRoot = SleFormat.FromValue("AccountRoot");
            Assert.NotNull(accountRoot);
        }
    }
}
