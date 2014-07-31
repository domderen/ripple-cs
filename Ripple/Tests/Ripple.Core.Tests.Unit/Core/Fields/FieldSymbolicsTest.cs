namespace Ripple.Core.Tests.Unit.Core.Fields
{
    using NUnit.Framework;

    using Ripple.Core.Core.Fields;

    [TestFixture]
    public class FieldSymbolicsTest
    {
        [Test]
        public void TestIsSymbolicField()
        {
            Assert.IsTrue(FieldSymbolics.IsSymbolicField(Field.LedgerEntryType));
        }

        [Test]
        public void TestAsInteger()
        {
            Assert.NotNull(FieldSymbolics.AsInteger(Field.LedgerEntryType, "AccountRoot"));
            Assert.AreEqual((int)'a', (int)FieldSymbolics.AsInteger(Field.LedgerEntryType, "AccountRoot"));
        }
    }
}
