// --------------------------------------------------------------------------------------------------------------------
// <copyright file="B58IdentifierTest.cs" company="">
//   
// </copyright>
// <summary>
//   The b 58 identifier test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ripple.Core.Tests.Unit.Encodings
{
    using NUnit.Framework;

    using Ripple.Core.Encodings;
    using Ripple.Core.Encodings.Base58;
    using Ripple.Core.Tests.Unit.Core;

    /// <summary>
    /// The b 58 identifier test.
    /// </summary>
    [TestFixture]
    public class B58IdentifierTest
    {
        /// <summary>
        /// The test decode family seed.
        /// </summary>
        [Test]
        public void testDecodeFamilySeed()
        {
            var b58 = new B58(Config.Config.DefaultAlphabet);
            var b58IdentiferCodecs = new B58IdentiferCodecs(b58);

            Assert.AreEqual(TestFixtures.MasterSeedBytes, b58IdentiferCodecs.DecodeFamilySeed(TestFixtures.MasterSeed));
        }

        /// <summary>
        /// The test encode family seed.
        /// </summary>
        [Test]
        public void testEncodeFamilySeed()
        {
            var b58 = new B58(Config.Config.DefaultAlphabet);
            var b58IdentiferCodecs = new B58IdentiferCodecs(b58);

            string masterSeedStringRebuilt = b58IdentiferCodecs.EncodeFamilySeed(TestFixtures.MasterSeedBytes);
            Assert.AreEqual(TestFixtures.MasterSeed, masterSeedStringRebuilt);
        }
    }
}