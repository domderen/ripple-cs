namespace Ripple.Core.Tests.Unit.Core
{
    using NUnit.Framework;

    using Ripple.Core.Core.Coretypes;
    using Ripple.Core.Encodings.Base58;
    using Ripple.Core.Tests.Unit.Core.Types;

    [TestFixture]
    public class AccountIdTest
    {
        private const string randomXqvWyhPcWjBE7nawXLTKH5YLNmSc = "randomXqvWyhPcWjBE7nawXLTKH5YLNmSc";

        [Test]
        public void TestAddress()
        {
            var account = AccountId.FromSeed(TestFixtures.MasterSeed);
            Assert.AreEqual(TestFixtures.MasterSeedAddress, account.Address);
        }

        [Test]
        public void TestBlackHoleAddy()
        {
            AccountId.FromAddress(randomXqvWyhPcWjBE7nawXLTKH5YLNmSc);
        }

        [Test]
        public void TestBlackGoleAddyCheckSumFail()
        {
            Assert.Throws<EncodingFormatException>(
                () => AccountId.FromAddress("R" + randomXqvWyhPcWjBE7nawXLTKH5YLNmSc.Substring(1)));
        }

        [Test]
        public void TestHashCode()
        {
            var a1 = AccountId.FromAddress(randomXqvWyhPcWjBE7nawXLTKH5YLNmSc);
            var a2 = AccountId.FromAddress(randomXqvWyhPcWjBE7nawXLTKH5YLNmSc);

            Assert.AreEqual(a1.GetHashCode(), a2.GetHashCode());
        }
    }
}
