namespace Ripple.Core.Tests.Unit.Core.Coretypes
{
    using Deveel.Math;

    using NUnit.Framework;

    using Ripple.Core.Core.Coretypes;
    using Ripple.Core.Core.Coretypes.Hash;

    [TestFixture]
    public class QualityTest
    {
        [Test]
        public void TestFromBookDirectory()
        {
            Hash256 hash256 =
                Hash256.OutTranslate.FromString("4627DFFCFF8B5A265EDBD8AE8C14A52325DBFEDAF4F5C32E5C08A1FB2E56F800");

            var value = BigDecimal.ValueOf(24.299);
            var quality = Quality.FromBookDirectory(hash256, true, false);

            var result = value.CompareTo(quality);

            Assert.AreEqual(0, result);
        }
    }
}
