namespace Ripple.Core.Tests.Unit.Core
{
    using NUnit.Framework;

    using Ripple.Core.Crypto.Ecdsa;
    using Ripple.Core.Tests.Unit.Core.Types;

    [TestFixture]
    public class SeedTest
    {
        [Test]
        public void TestPassphraseParsing()
        {
            Assert.AreEqual(TestFixtures.MasterSeed, PhraseToFamilySeed("masterpassphrase"));
            // These were taken from rippled wallet propose
            Assert.AreEqual("ssbTMHrmEJP7QEQjWJH3a72LQipBM", PhraseToFamilySeed("alice"));
            Assert.AreEqual("spkcsko6Ag3RbCSVXV2FJ8Pd4Zac1", PhraseToFamilySeed("bob"));
            Assert.AreEqual("snzb83cV8zpLPTE4nQamoLP9pbhB7", PhraseToFamilySeed("carol"));
            Assert.AreEqual("snczogzwPXNMFq6YPBE7SUwqzkWih", PhraseToFamilySeed("dan"));
            Assert.AreEqual("snsBvdoBXhMYYUnabGieeBFWEdqRM", PhraseToFamilySeed("bitstamp"));
            Assert.AreEqual("saDGZcfdL21t9gtoa3JiNUmMVReaS", PhraseToFamilySeed("mtgox"));
            Assert.AreEqual("spigbKN5chn5wWyE8dvTN9wH36Ff1", PhraseToFamilySeed("amazon"));
        }

        public static string PhraseToFamilySeed(string passphrase)
        {
            return Config.Config.B58IdentiferCodecs.EncodeFamilySeed(Seed.PassPhraseToSeedBytes(passphrase));
        }
    }
}
