// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyPairTest.cs" company="">
//   
// </copyright>
// <summary>
//   The key pair test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Ripple.Core.Tests.Unit.Core
{
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    using Org.BouncyCastle.Utilities.Encoders;

    using Ripple.Core.Core.Coretypes;
    using Ripple.Core.Crypto.Ecdsa;
    using Ripple.Core.Tests.Unit.Core.Types;

    /// <summary>
    /// The key pair test.
    /// </summary>
    [TestFixture]
    public class KeyPairTest
    {
        /// <summary>
        /// The key pair.
        /// </summary>
        private IKeyPair keyPair = Seed.CreateKeyPair(TestFixtures.MasterSeedBytes);

        /// <summary>
        /// The test varify.
        /// </summary>
        [Test]
        public void testVarify()
        {
            Assert.True(this.keyPair.Verify(TestFixtures.MasterSeedBytes, Hex.Decode(TestFixtures.SingedMasterSeedBytes)));
        }

        /// <summary>
        /// The sanity test sign and varify.
        /// </summary>
        [Test]
        public void sanityTestSignAndVarify()
        {
            Assert.True(this.keyPair.Verify(TestFixtures.MasterSeedBytes, this.keyPair.Sign(TestFixtures.MasterSeedBytes)));
        }

        /// <summary>
        /// The test derivation from seed bytes.
        /// </summary>
        [Test]
        public void testDerivationFromSeedBytes()
        {
            Assert.AreEqual("0330E7FC9D56BB25D6893BA3F317AE5BCF33B3291BD63DB32654A313222F7FD020", this.keyPair.PubHex());
            Assert.AreEqual("1ACAAEDECE405B2A958212629E16F2EB46B153EEE94CDD350FDEFF52795525B7", this.keyPair.PrivHex());
        }

        /// <summary>
        /// The test derivation from string.
        /// </summary>
        [Test]
        public void testDerivationFromString()
        {
            IKeyPair keyPairFromSeed = AccountId.KeyPairFromSeed(TestFixtures.MasterSeed);
            Assert.AreEqual(
                "0330E7FC9D56BB25D6893BA3F317AE5BCF33B3291BD63DB32654A313222F7FD020", 
                keyPairFromSeed.PubHex());
            Assert.AreEqual(
                "1ACAAEDECE405B2A958212629E16F2EB46B153EEE94CDD350FDEFF52795525B7", 
                keyPairFromSeed.PrivHex());
        }

        /// <summary>
        /// The get file text.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string getFileText(string filename)
        {
            StringBuilder b = new StringBuilder();

            foreach (var line in File.ReadLines(filename))
            {
                b.AppendLine(line);
            }

            return b.ToString();
        }

        /// <summary>
        /// The test ripple lib garbage.
        /// </summary>
        [Test]
        public void testRippleLibGarbage()
        {
            //string text = getFileText("/home/nick/ripple-lib/dumps.json");

            //JArray array = JArray.Parse(text);

            //AccountId root = AccountId.FromString("root");
            //IKeyPair kp = root.KeyPair;
            //var zeros = new byte[32];

            //for (int i = 0; i < array.Count; i++)
            //{
            //    string sig = array[i].ToString();
            //    byte[] sigBytes = B16.Decode(sig);
            //    Assert.True(KeyPair.IsStrictlyCanonical(sigBytes));
            //    Assert.True(kp.Verify(zeros, sigBytes));
            //}
        }
    }
}