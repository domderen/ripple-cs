// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JSEscapeTest.cs" company="">
//   
// </copyright>
// <summary>
//   The js escape test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Ripple.Core.Tests.Unit.Crypto.SjclJson
{
    using System;

    using NUnit.Framework;

    using Ripple.Core.Crypto.Sjcljson;

    /// <summary>
    /// The js escape test.
    /// </summary>
    [TestFixture]
    public class JSEscapeTest
    {
        /// <summary>
        /// The test encodings.
        /// </summary>
        [Test]
        public void testEncodings()
        {
            assertSane("エォオカガキ", "%u30A8%u30A9%u30AA%u30AB%u30AC%u30AD");
            assertSane("wtf bbq?? -- this", "wtf%20bbq%3F%3F%20--%20this");
        }

        /// <summary>
        /// The assert sane.
        /// </summary>
        /// <param name="raw">
        /// The raw.
        /// </param>
        /// <param name="escaped">
        /// The escaped.
        /// </param>
        private void assertSane(string raw, string escaped)
        {
            Assert.AreEqual(raw, JsEscape.Unescape(escaped));
            Assert.AreEqual(escaped, JsEscape.Escape(raw));
        }
    }
}