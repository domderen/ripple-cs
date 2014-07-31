namespace Ripple.Core.Tests.Unit.Core.Types.Known.Sle
{
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using Ripple.Core.Core.Coretypes;
    using Ripple.Core.Core.Types.Known.Sle.Entries;

    [TestFixture]
    public class OfferTest
    {
        [Test]
        public void TestHarness()
        {
            var offerJson = JObject.Parse("{\"Account\": \"rEssC7sBh8ZCytFXBW4jpGfcb8XMRTgUmT\"," +
                " \"BookDirectory\": \"4627DFFCFF8B5A265EDBD8AE8C14A52325DBFEDAF4F5C32E5D05540F663BF000\"," +
                " \"BookNode\": \"0000000000000000\"," +
                " \"Flags\": 131072," +
                " \"LedgerEntryType\": \"Offer\"," +
                " \"OwnerNode\": \"0000000000000000\"," +
                " \"PreviousTxnID\": \"1E94B1C1CCAA94F92F43546833522D7CA7259AF42E0FC9B4FD1F1CD322EC2529\"," +
                " \"PreviousTxnLgrSeq\": 3441312," +
                " \"Sequence\": 86," +
                " \"TakerGets\": {\"currency\": \"USD\"," +
                "               \"issuer\": \"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"," +
                "               \"value\": \"79\"}," +
                " \"TakerPays\": \"11848420000\"," +
                " \"index\": \"630579D43A800A7B8DE70F85C40536640DF94678A8018818BAD2986A17B1DC0A\"," +
                " \"quality\": \"149980000\"}");

            Offer offer = (Offer)StObject.FromJObject(offerJson);

            Assert.AreEqual(offer.AskQuality, Amount.FromString(offerJson.GetValue("quality").ToString()).Value);
            Assert.AreEqual("USD", offer.GetsOne.CurrencyString);
            Assert.AreEqual("XRP", offer.PaysOne.CurrencyString);
        }
    }
}
