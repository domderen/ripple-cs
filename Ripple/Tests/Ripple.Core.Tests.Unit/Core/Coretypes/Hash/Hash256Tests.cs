using NUnit.Framework;
using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;

namespace Ripple.Core.Tests.Unit.Core.Coretypes.Hash
{
    [TestFixture]
    public class Hash256Tests
    {
        [Test]
        public void TestAccountIdLedgerIndex()
        {
            const string addy = "rP1coskQzayaQ9geMdJgAV5f3tNZcHghzH";
            const string ledgerIndex = "D66D0EC951FD5707633BEBE74DB18B6D2DDA6771BA0FBF079AD08BFDE6066056";
            Hash256 expectedLedgerIndex = Hash256.Translate.FromString(ledgerIndex);
            AccountId accountId = AccountId.FromAddress(addy);
            Hash256 builtLedgerIndex = Hash256.AccountIdLedgerIndex(accountId);
            Assert.AreEqual(expectedLedgerIndex, builtLedgerIndex);
        }
    }
}
