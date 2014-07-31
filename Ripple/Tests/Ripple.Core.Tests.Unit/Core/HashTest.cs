// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashTest.cs" company="">
//   
// </copyright>
// <summary>
//   The hash test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ripple.Core.Tests.Unit.Core
{
    using System;

    using NUnit.Framework;

    using Ripple.Core.Core.Coretypes;
    using Ripple.Core.Core.Coretypes.Hash;

    /// <summary>
    /// The hash test.
    /// </summary>
    [TestFixture]
    public class HashTest
    {
        /// <summary>
        /// The test padding.
        /// </summary>
        [Test]
        public void TestPadding()
        {
            var hash128 = new Hash128(new byte[] { 0 });
            Assert.AreEqual(16, hash128.Bytes.Length);
        }

        /// <summary>
        /// The test bounds checking.
        /// </summary>
        [Test]
        public void testBoundsChecking()
        {
            Assert.Throws<ApplicationException>(() => new Hash128(new byte[32]));
        }

        /// <summary>
        /// The test tree map suitability.
        /// </summary>
        [Test]
        public void testTreeMapSuitability()
        {
            Hash256 a = new Hash256(new byte[32]);
            Hash256 b = new Hash256(new byte[32]);
            Hash256 c = new Hash256(new byte[32]);
            Hash256 d = new Hash256(new byte[32]);

            StObject objectA = new StObject();
            StObject objectB = new StObject();
            StObject objectC = new StObject();

            a.Bytes[0] = (byte)'a';
            b.Bytes[0] = (byte)'b';
            c.Bytes[0] = (byte)'c';
            d.Bytes[0] = (byte)'a';

            var tree = new Hash256.Hash256Map<StObject> { { a, objectA } };

            // There can be ONLY one
            Assert.True(tree.ContainsKey(d));

            tree.Add(b, objectB);
            tree.Add(c, objectC);

            Assert.True(tree[a] == objectA);
            Assert.True(tree[b] == objectB);
            Assert.True(tree[c] == objectC);
        }
    }
}