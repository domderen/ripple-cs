// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmountTest.cs" company="">
//   
// </copyright>
// <summary>
//   The amount test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Ripple.Core.Tests.Unit.Core
{
    using System;

    using Deveel.Math;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using Org.BouncyCastle.Utilities;

    using Ripple.Core.Core.Coretypes;
    using Ripple.Core.Encodings.Base58;
    using Ripple.Core.Tests.Unit.Core.Types;

    using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

    /// <summary>
    /// The amount test.
    /// </summary>
    [TestFixture]
    public class AmountTest
    {
        /// <summary>
        /// The root Address.
        /// </summary>
        private string rootAddress = TestFixtures.MasterSeedAddress;

        /// <summary>
        /// The out amounts.
        /// </summary>
        private Amount.OutTranslator outAmounts = Amount.OutTranslate;

        /// <summary>
        /// The in amounts.
        /// </summary>
        private Amount.InTranslator inAmounts = Amount.InTranslate;

        /// <summary>
        /// The a 50.
        /// </summary>
        public Amount a50 = Amt("50/USD/root"); // on the fly, cached, `passphrase to Address`

        /// <summary>
        /// The b 20.
        /// </summary>
        public Amount b20 = Amt("20/USD");

        /// <summary>
        /// The c 5.
        /// </summary>
        public Amount c5 = Amt("5/USD");

        /// <summary>
        /// The test xrp from mantissa bytes.
        /// </summary>
        [Test]
        public void TestXrpFromMantissaBytes()
        {
            long l = 99000001;
            byte[] mantissa = new UInt32(l).ToByteArray();

            var bigDecimal = Amount.XrpFromDropsMantissa(mantissa, 1);
            Assert.AreEqual("99000001", bigDecimal.ScaleByPowerOfTen(6).ToPlainString());
        }

        /// <summary>
        /// The test funky currencies.
        /// </summary>
        [Test]
        public void TestFunkyCurrencies()
        {
            const string AmtJson =
                "{\"currency\": \"015841551A748AD23FEFFFFFFFEA028000000000\","
                + "\"issuer\": \"rM1oqKtfh1zgjdAgbFmaRm3btfGBX25xVo\"," + "\"value\": \"1000\"}";

            Amount Amt = Amount.OutTranslate.FromJObject(JObject.Parse(AmtJson));
            const string Expected =
                "D5438D7EA4C68000015841551A748AD23FEFFFFFFFEA028000000000E4FE687C90257D3D2D694C8531CDEECBE84F3367";
            string hex = Amt.ToHex();

            Assert.AreEqual(Expected, hex);
        }

        /// <summary>
        /// The test abs.
        /// </summary>
        [Test]
        public void TestAbs()
        {
            Assert.AreEqual(Amt("-11").Abs(), Amt("11"));
        }

        /// <summary>
        /// The test subtraction.
        /// </summary>
        [Test]
        public void TestSubtraction()
        {
            Assert.AreEqual(Amt("10"), Amt("11").Subtract(Amount.FromString("0.000001")));
            Assert.AreEqual(Amt("10"), Amt("11").Subtract(new BigDecimal("0.000001")));
        }

        /// <summary>
        /// The test serializing 0 xrp.
        /// </summary>
        [Test]
        public void TestSerializing0Xrp()
        {
            var Amt = Amount.FromDropString("0");
            var s = this.inAmounts.ToHex(Amt);
            Assert.AreEqual("4000000000000000", s);
            Assert.AreEqual(Amount.BinaryFlagIsNonNegativeNative.ToString(16), s);
        }

        /// <summary>
        /// The test serializing negative iou.
        /// </summary>
        [Test]
        public void TestSerializingNegativeIou()
        {
            const string Json =
                "{\"currency\": \"USD\", \"issuer\": \"rrrrrrrrrrrrrrrrrrrrBZbvji\", \"value\": \"-99.2643419677474\"}";

            var amount = this.outAmounts.FromJObject(JObject.Parse(Json));
            var hex = this.inAmounts.ToHex(amount);

            var offset = amount.Offset;

            Assert.AreEqual(-14, offset);
            Assert.IsTrue(amount.IsNegative);
            Assert.IsFalse(amount.IsNative);

            const string ExpectedHex =
                "94E3440A102F5F5400000000000000000000000055534400000000000000000000000000000000000000000000000001";

            Assert.AreEqual(ExpectedHex, hex);
        }

        /// <summary>
        /// The test xrpiou legacy support.
        /// </summary>
        [Test]
        public void testXRPIOULegacySupport()
        {
            const string Json =
                "{\n" + "  \"currency\": \"0000000000000000000000005852500000000000\",\n"
                + "  \"issuer\": \"rrrrrrrrrrrrrrrrrrrrBZbvji\",\n" + "  \"value\": \"0\"\n" + "}";

            var amount = this.outAmounts.FromJObject(JObject.Parse(Json));
            Assert.IsFalse(amount.IsNative);

            var jsonObject = this.inAmounts.ToJObject(amount);
            var rebuilt = this.outAmounts.FromJObject(jsonObject);
            Assert.AreEqual(amount, rebuilt);

            var a1Bytes = this.inAmounts.ToBytes(amount);
            var a2Bytes = this.inAmounts.ToBytes(rebuilt);

            bool Equals = Arrays.AreEqual(a1Bytes, a2Bytes);
            Assert.True(Equals);

            string Legacy = "{\n" + "    \"currency\": \"0000000000000000000000005852500000000000\",\n"
                            + "    \"issuer\": \"rrrrrrrrrrrrrrrrrrrrBZbvji\",\n" + "    \"value\": \"0\"\n" + "  }\n"
                            + "  \n" + string.Empty;

            const string ExpectedHex =
                "800000000000000000000000000000000000000058525000000000000000000000000000000000000000000000000001";

            Amount legacyAmount = this.outAmounts.FromJObject(JObject.Parse(Legacy));
            Assert.AreEqual(ExpectedHex, this.inAmounts.ToHex(legacyAmount));
        }

        /// <summary>
        /// The test decimal parsing.
        /// </summary>
        [Test]
        public void TestDecimalParsing()
        {
            Assert.AreEqual(0, Amt("1.0").CompareTo(Amt("1000000")));
            Assert.AreEqual(0, Amt("1").CompareTo(Amt("0.000001")));
        }

        /// <summary>
        /// The tests mother fucker do you write them.
        /// </summary>
        [Test]
        public void TestsMotherFuckerDoYouWriteThem()
        {
            Assert.Throws<PrecisionException>(() => Amt("-0.0001621621623423423234234234"));
        }

        /// <summary>
        /// The test scale assumption.
        /// </summary>
        [Test]
        public void TestScaleAssumption()
        {
            this.AssertScale("1.0", 0);
            this.AssertScale("1.000", 0);
            this.AssertScale("1", 0);
        }

        /// <summary>
        /// The test decimal equality.
        /// </summary>
        [Test]
        public void TestDecimalEquality()
        {
            /*This is something to watch log for! Must delegate to compareTo() !*/
            Assert.False(this.MyDecimal("1.0").Equals(this.MyDecimal("1.00")));
        }

        /// <summary>
        /// The test offer.
        /// </summary>
        [Test]
        public void TestOffset()
        {
            this.AssertOffset(".9999999999999999", -16);
            this.AssertOffset("9.999999999999999", -15);
            this.AssertOffset("99.99999999999999", -14);
            this.AssertOffset("999.9999999999999", -13);
            this.AssertOffset("9999.999999999999", -12);
            this.AssertOffset("99999.99999999999", -11);
            this.AssertOffset("999999.9999999999", -10);
            this.AssertOffset("9999999.999999999", -9);
            this.AssertOffset("99999999.99999999", -8);
            this.AssertOffset("999999999.9999999", -7);
            this.AssertOffset("9999999999.999999", -6);
            this.AssertOffset("99999999999.99999", -5);
            this.AssertOffset("999999999999.9999", -4);
            this.AssertOffset("9999999999999.999", -3);
            this.AssertOffset("99999999999999.99", -2);
            this.AssertOffset("999999999999999.9", -1);

            this.AssertOffset(".9", -16);

            this.AssertOffset("9", -15);
            this.AssertOffset("99", -14);
            this.AssertOffset("999", -13);
            this.AssertOffset("9999", -12);
            this.AssertOffset("99999", -11);
            this.AssertOffset("999999", -10);
            this.AssertOffset("9999999", -9);
            this.AssertOffset("99999999", -8);
            this.AssertOffset("999999999", -7);
            this.AssertOffset("9999999999", -6);
            this.AssertOffset("99999999999", -5);
            this.AssertOffset("999999999999", -4);
            this.AssertOffset("9999999999999", -3);
            this.AssertOffset("99999999999999", -2);
            this.AssertOffset("999999999999999", -1);
            this.AssertOffset("9999999999999999", 0);
        }

        /// <summary>
        /// The test ridiculous ness.
        /// </summary>
        [Test]
        public void TestRidiculousNess()
        {
            Amount oneXRP = Amt("1.0");
            Amount oneUSD = Amt("1.0/USD");
            Assert.AreEqual(oneUSD, oneUSD.Multiply(oneXRP));
        }

        /// <summary>
        /// The test division.
        /// </summary>
        [Test]
        public void TestDivision()
        {
            Amount d = this.a50.Divide(this.b20).Multiply(this.c5);
            Assert.AreEqual("50/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", this.a50.StringRepr());
            Assert.AreEqual("12.5/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", d.StringRepr());
        }

        /// <summary>
        /// The test Addition.
        /// </summary>
        [Test]
        public void TestAddition()
        {
            Amount d = this.a50.Add(this.b20).Add(this.c5);
            Assert.AreEqual("75/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", d.StringRepr());
            Assert.AreEqual("80/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", d.Add(new BigInteger("5")).StringRepr());
            Assert.AreEqual("80/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", d.Add(5).StringRepr());
        }

        /// <summary>
        /// The test min max.
        /// </summary>
        [Test]
        public void TestMinMax()
        {
            Amount d75 = this.a50.Add(this.b20).Add(this.c5);

            Assert.AreEqual(this.a50.Min(this.b20), this.b20);
            Assert.AreEqual(this.b20.Min(this.c5), this.c5);
            Assert.AreEqual(this.b20.Max(d75), d75);
            Assert.AreEqual(this.b20.Max(this.c5), this.b20);
            Assert.AreEqual(Amt("-5/USD").Max(this.c5), this.c5);
        }

        /// <summary>
        /// The test equals.
        /// </summary>
        [Test]
        public void TestEquals()
        {
            Assert.True(this.a50.Equals(Amt("50/USD/root")));
            Assert.False(this.a50.Equals(Amt("50/USD/bob")));
            Assert.True(this.a50.EqualsExceptIssuer(Amt("50/USD/bob")));
        }

        /// <summary>
        /// The test iou parsing.
        /// </summary>
        [Test]
        public void TestIouParsing()
        {
            Assert.AreEqual("USD", Amt("1.0/USD").CurrencyString);
            Amount amount = Amt("1.0/USD/" + TestFixtures.MasterSeedAddress);
            Assert.AreEqual("USD", amount.CurrencyString);
            Assert.AreEqual(TestFixtures.MasterSeedAddress, amount.IssuerString);
            Assert.AreEqual(false, amount.IsNative);
        }

        /// <summary>
        /// The test iou issuer validation.
        /// </summary>
        [Test]
        public void TestIouIssuerValidation()
        {
            Assert.Throws<EncodingFormatException>(() => Amt("1.0/USD/" + TestFixtures.RootAccount + "F"));
        }

        /// <summary>
        /// The test check whole.
        /// </summary>
        [Test]
        public void TestCheckWhole()
        {
            Assert.Throws<ApplicationException>(() => Amount.CheckDropsValueWhole("1.0"));
        }

        /// <summary>
        /// The test zero usd equals zero usd.
        /// </summary>
        [Test]
        public void TestZeroUsdEqualsZeroUsd()
        {
            Amount a = Amt("0/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("0/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test zero usd equals negative zero usd.
        /// </summary>
        [Test]
        public void TestZeroUsdEqualsNegativeZeroUsd()
        {
            Amount a = Amt("0/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("-0/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test zero xrp equals zero xrp.
        /// </summary>
        [Test]
        public void TestZeroXrpEqualsZeroXrp()
        {
            Amount a = Amt("0");
            Amount b = Amt("0.0");
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test zero xrp equals negative zero xrp.
        /// </summary>
        [Test]
        public void TestZeroXrpEqualsNegativeZeroXrp()
        {
            Amount a = Amt("0");
            Amount b = Amt("-0");
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test 10 usd equals 10 usd.
        /// </summary>
        [Test]
        public void Test10UsdEquals10Usd()
        {
            Amount a = Amt("10/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("10/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test equality of usd with fraction.
        /// </summary>
        [Test]
        public void TestEqualityOfUsdWithFraction()
        {
            Amount a = Amt("123.4567/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("123.4567/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test 10 drops equals 10 drops.
        /// </summary>
        [Test]
        public void Test10DropsEquals10Drops()
        {
            Amount a = Amt("10");
            Amount b = Amt("10");
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test fractional xrp equality.
        /// </summary>
        [Test]
        public void TestFractionalXrpEquality()
        {
            Amount a = Amt("1.1");
            Amount b = Amt("11.0").Divide(10);
            Assert.True(a.Equals(b));
        }

        /// <summary>
        /// The test equality ignoring issuer.
        /// </summary>
        [Test]
        public void TestEqualityIgnoringIssuer()
        {
            Amount a = Amt("0/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("0/USD/rH5aWQJ4R7v4Mpyf4kDBUvDFT5cbpFq3XP");
            Assert.True(a.EqualsExceptIssuer(b));
        }

        /// <summary>
        /// The test trailing zeros equality ignoring issuer.
        /// </summary>
        [Test]
        public void TestTrailingZerosEqualityIgnoringIssuer()
        {
            Amount a = Amt("1.1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("1.10/USD/rH5aWQJ4R7v4Mpyf4kDBUvDFT5cbpFq3XP");
            Assert.True(a.EqualsExceptIssuer(b));
        }

        /// <summary>
        /// The test iou exponent mismatch.
        /// </summary>
        [Test]
        public void TestIouExponentMismatch()
        {
            Amount a = Amt("10/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("100/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test xrp exponent mismatch.
        /// </summary>
        [Test]
        public void TestXrpExponentMismatch()
        {
            Amount a = Amt("10");
            Amount b = Amt("100");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ mantissa_ mismatch_ one_ io u_ not_ equaling_ two.
        /// </summary>
        [Test]
        public void test_Mantissa_Mismatch_One_IOU_Not_Equaling_Two()
        {
            Amount a = Amt("1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("2/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ mantissa_ mismatch_ one_ xr p_ not_ equaling_ two.
        /// </summary>
        public void test_Mantissa_Mismatch_One_XRP_Not_Equaling_Two()
        {
            Amount a = Amt("1");
            Amount b = Amt("2");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ negativity_ in_ equality_ for_ iou.
        /// </summary>
        public void test_Negativity_In_Equality_For_IOU()
        {
            Amount a = Amt("1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("-1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ negativity_ in_ equality_ for_ xrp.
        /// </summary>
        [Test]
        public void test_Negativity_In_Equality_For_XRP()
        {
            Amount a = Amt("1");
            Amount b = Amt("-1");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ issuer_ derived_ inequality.
        /// </summary>
        [Test]
        public void test_Issuer_Derived_Inequality()
        {
            Amount a = Amt("1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("1/USD/rH5aWQJ4R7v4Mpyf4kDBUvDFT5cbpFq3XP");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ currency_ inequality.
        /// </summary>
        [Test]
        public void test_Currency_Inequality()
        {
            Amount a = Amt("1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("1/EUR/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ same_ value_ yet_ native_ vs_ io u_ inequality.
        /// </summary>
        [Test]
        public void test_Same_Value_Yet_Native_Vs_IOU_Inequality()
        {
            Amount a = Amt("1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("1");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ same_ value_ yet_ native_ vs_ io u_ inequality_ operand_ switch.
        /// </summary>
        [Test]
        public void test_Same_Value_Yet_Native_Vs_IOU_Inequality_Operand_Switch()
        {
            Amount a = Amt("1");
            Amount b = Amt("1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test mantissa mismatch fractional iou.
        /// </summary>
        [Test]
        public void TestMantissaMismatchFractionalIou()
        {
            Amount a = Amt("0.1/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Amount b = Amt("0.2/USD/rNDKeo9RrCiRdfsMG8AdoZvNZxHASGzbZL");
            Assert.False(a.Equals(b));
        }

        /// <summary>
        /// The test_ negate_native_123.
        /// </summary>
        [Test]
        public void test_Negate_native_123()
        {
            Assert.AreEqual("-0.000123/XRP", Amt("123").Negate().ToTextFull());
        }

        /// <summary>
        /// The test_ negate_native_123_2.
        /// </summary>
        [Test]
        public void test_Negate_native_123_2()
        {
            Assert.AreEqual("0.000123/XRP", Amt("-123").Negate().ToTextFull());
        }

        /// <summary>
        /// The test_ negate_non_native_123.
        /// </summary>
        [Test]
        public void test_Negate_non_native_123()
        {
            Assert.AreEqual("-123/USD/" + this.rootAddress, Amt("123/USD/root").Negate().ToTextFull());
        }

        /// <summary>
        /// The test_ negate_non_native_123_2.
        /// </summary>
        [Test]
        public void test_Negate_non_native_123_2()
        {
            Assert.AreEqual("123/USD/" + this.rootAddress, Amt("-123/USD/root").Negate().ToTextFull());
        }

        // [Test]
        // public void test_Clone_non_native_123_3() {
        // Assert.AreEqual("-123/USD/" + rootAddress, Amt("-123/USD/root").clone().ToTextFull());
        // }
        /// <summary>
        /// The test_ add_ xr p_to_ xrp.
        /// </summary>
        [Test]
        public void test_Add_XRP_to_XRP()
        {
            Assert.AreEqual("0.0002/XRP", Amt("150").Add(Amt("50")).ToTextFull());
        }

        /// <summary>
        /// The test_ add_ us d_to_ usd.
        /// </summary>
        [Test]
        public void test_Add_USD_to_USD()
        {
            Assert.AreEqual("200.52/USD/" + this.rootAddress, Amt("150.02/USD/root").Add(Amt("50.5/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_0_ xr p_with_0_ xrp.
        /// </summary>
        [Test]
        public void test_Multiply_0_XRP_with_0_XRP()
        {
            Assert.AreEqual("0/XRP", Amt("0").Multiply(Amt("0")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_0_ us d_with_0_ xrp.
        /// </summary>
        [Test]
        public void test_Multiply_0_USD_with_0_XRP()
        {
            Assert.AreEqual("0/USD/" + this.rootAddress, Amt("0/USD/root").Multiply(Amt("0")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_0_ xr p_with_0_ usd.
        /// </summary>
        [Test]
        public void test_Multiply_0_XRP_with_0_USD()
        {
            Assert.AreEqual("0/XRP", Amt("0").Multiply(Amt("0/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_1_ xr p_with_0_ xrp.
        /// </summary>
        [Test]
        public void test_Multiply_1_XRP_with_0_XRP()
        {
            Assert.AreEqual("0/XRP", Amt("1").Multiply(Amt("0")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_1_ us d_with_0_ xrp.
        /// </summary>
        [Test]
        public void test_Multiply_1_USD_with_0_XRP()
        {
            Assert.AreEqual("0/USD/" + this.rootAddress, Amt("1/USD/root").Multiply(Amt("0")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_1_ xr p_with_0_ usd.
        /// </summary>
        [Test]
        public void test_Multiply_1_XRP_with_0_USD()
        {
            Assert.AreEqual("0/XRP", Amt("1").Multiply(Amt("0/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_0_ xr p_with_1_ xrp.
        /// </summary>
        [Test]
        public void test_Multiply_0_XRP_with_1_XRP()
        {
            Assert.AreEqual("0/XRP", Amt("0").Multiply(Amt("1")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_0_ us d_with_1_ xrp.
        /// </summary>
        [Test]
        public void test_Multiply_0_USD_with_1_XRP()
        {
            Assert.AreEqual("0/USD/" + this.rootAddress, Amt("0/USD/root").Multiply(Amt("1")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_0_ xr p_with_1_ usd.
        /// </summary>
        [Test]
        public void test_Multiply_0_XRP_with_1_USD()
        {
            Assert.AreEqual("0/XRP", Amt("0").Multiply(Amt("1/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ xr p_with_ usd.
        /// </summary>
        [Test]
        public void test_Multiply_XRP_with_USD()
        {
            Assert.AreEqual("2000/XRP", Amt("200.0").Multiply(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ xr p_with_ us d 2.
        /// </summary>
        [Test]
        public void test_Multiply_XRP_with_USD2()
        {
            Assert.AreEqual("0.2/XRP", Amt("20000").Multiply(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ xr p_with_ us d 3.
        /// </summary>
        [Test]
        public void test_Multiply_XRP_with_USD3()
        {
            Assert.AreEqual("20/XRP", Amt("2000000").Multiply(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ xr p_with_ us d_neg.
        /// </summary>
        [Test]
        public void test_Multiply_XRP_with_USD_neg()
        {
            Assert.AreEqual("-0.002/XRP", Amt("200").Multiply(Amt("-10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ xr p_with_ us d_neg_frac.
        /// </summary>
        [Test]
        public void test_Multiply_XRP_with_USD_neg_frac()
        {
            Assert.AreEqual("-0.222/XRP", Amt("-0.006").Multiply(Amt("37/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ us d_with_ usd.
        /// </summary>
        [Test]
        public void test_Multiply_USD_with_USD()
        {
            Assert.AreEqual("20000/USD/" + this.rootAddress, Amt("2000/USD/root").Multiply(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ us d_with_ us d 2.
        /// </summary>
        [Test]
        public void test_Multiply_USD_with_USD2()
        {
            Assert.AreEqual(
                "200000000000/USD/" + this.rootAddress, 
                Amt("2000000/USD/root").Multiply(Amt("100000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ eu r_with_ us d_result_1.
        /// </summary>
        [Test]
        public void test_Multiply_EUR_with_USD_result_1()
        {
            Assert.AreEqual(
                "100000/EUR/" + this.rootAddress, 
                Amt("100/EUR/root").Multiply(Amt("1000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ eu r_with_ us d_neg.
        /// </summary>
        [Test]
        public void test_Multiply_EUR_with_USD_neg()
        {
            Assert.AreEqual(
                "-48000000/EUR/" + this.rootAddress, 
                Amt("-24000/EUR/root").Multiply(Amt("2000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ eu r_with_ us d_neg_1.
        /// </summary>
        [Test]
        public void test_Multiply_EUR_with_USD_neg_1()
        {
            Assert.AreEqual("-100/EUR/" + this.rootAddress, Amt("0.1/EUR/root").Multiply(Amt("-1000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ eu r_with_ xr p_factor_1.
        /// </summary>
        [Test]
        public void test_Multiply_EUR_with_XRP_factor_1()
        {
            Assert.AreEqual("0.0001/EUR/" + this.rootAddress, Amt("0.05/EUR/root").Multiply(Amt("0.002000")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ eu r_with_ xr p_neg.
        /// </summary>
        [Test]
        public void test_Multiply_EUR_with_XRP_neg()
        {
            Assert.AreEqual("-0.0005/EUR/" + this.rootAddress, Amt("-100/EUR/root").Multiply(Amt("0.000005")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ eu r_with_ xr p_neg_1.
        /// </summary>
        [Test]
        public void test_Multiply_EUR_with_XRP_neg_1()
        {
            Assert.AreEqual("-0.0001/EUR/" + this.rootAddress, Amt("-0.05/EUR/root").Multiply(Amt("0.002000")).ToTextFull());
        }

        /// <summary>
        /// The test_ multiply_ xr p_with_ xrp.
        /// </summary>
        [Test]
        public void test_Multiply_XRP_with_XRP()
        {
            // This is actually too small for XRP so is rounded into nothingness
            // TODO, rounding values that are inside bound extremes seems fine
            // but rounding to nothingness ?? Should that blow up ??
            Assert.AreEqual("0/XRP", Amt("10").Multiply(Amt("10")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ xr p_by_ usd.
        /// </summary>
        [Test]
        public void test_Divide_XRP_by_USD()
        {
            Assert.AreEqual("0.00002/XRP", Amt("200").Divide(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ xr p_by_ us d 2.
        /// </summary>
        [Test]
        public void test_Divide_XRP_by_USD2()
        {
            Assert.AreEqual("0.002/XRP", Amt("20000").Divide(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ xr p_by_ us d 3.
        /// </summary>
        [Test]
        public void test_Divide_XRP_by_USD3()
        {
            Assert.AreEqual("0.2/XRP", Amt("2000000").Divide(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ xr p_by_ us d_neg.
        /// </summary>
        [Test]
        public void test_Divide_XRP_by_USD_neg()
        {
            Assert.AreEqual("-0.00002/XRP", Amt("200").Divide(Amt("-10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ xr p_by_ us d_neg_frac.
        /// </summary>
        [Test]
        public void test_Divide_XRP_by_USD_neg_frac()
        {
            Assert.AreEqual("-0.000162/XRP", Amt("-6000").Divide(Amt("37/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ us d_by_ usd.
        /// </summary>
        [Test]
        public void test_Divide_USD_by_USD()
        {
            Assert.AreEqual("200/USD/" + this.rootAddress, Amt("2000/USD/root").Divide(Amt("10/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ us d_by_ us d_fractional.
        /// </summary>
        [Test]
        public void test_Divide_USD_by_USD_fractional()
        {
            Assert.AreEqual(
                "57142.85714285714/USD/" + this.rootAddress, 
                Amt("2000000/USD/root").Divide(Amt("35/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ us d_by_ us d 2.
        /// </summary>
        [Test]
        public void test_Divide_USD_by_USD2()
        {
            Assert.AreEqual(
                "20/USD/" + this.rootAddress, 
                Amt("2000000/USD/root").Divide(Amt("100000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ eu r_by_ us d_factor_1.
        /// </summary>
        [Test]
        public void test_Divide_EUR_by_USD_factor_1()
        {
            Assert.AreEqual("0.1/EUR/" + this.rootAddress, Amt("100/EUR/root").Divide(Amt("1000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ eu r_by_ us d_neg.
        /// </summary>
        [Test]
        public void test_Divide_EUR_by_USD_neg()
        {
            Assert.AreEqual("-12/EUR/" + this.rootAddress, Amt("-24000/EUR/root").Divide(Amt("2000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ eu r_by_ us d_neg_1.
        /// </summary>
        [Test]
        public void test_Divide_EUR_by_USD_neg_1()
        {
            Assert.AreEqual("-0.1/EUR/" + this.rootAddress, Amt("100/EUR/root").Divide(Amt("-1000/USD/root")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ eu r_by_ xr p_result_1.
        /// </summary>
        [Test]
        public void test_Divide_EUR_by_XRP_result_1()
        {
            Assert.AreEqual("50000/EUR/" + this.rootAddress, Amt("100/EUR/root").Divide(Amt("0.002000")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ eu r_by_ xr p_neg.
        /// </summary>
        [Test]
        public void test_Divide_EUR_by_XRP_neg()
        {
            Assert.AreEqual("-20000000/EUR/" + this.rootAddress, Amt("-100/EUR/root").Divide(Amt("0.000005")).ToTextFull());
        }

        /// <summary>
        /// The test_ divide_ eu r_by_ xr p_neg_1.
        /// </summary>
        [Test]
        public void test_Divide_EUR_by_XRP_neg_1()
        {
            Assert.AreEqual("-50000/EUR/" + this.rootAddress, Amt("-100/EUR/root").Divide(Amt("0.002000")).ToTextFull());
        }

        /// <summary>
        /// The test_ parse_native_0.
        /// </summary>
        [Test]
        public void test_Parse_native_0()
        {
            Assert.AreEqual("0/XRP", Amt("0").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_native_0_pt_0.
        /// </summary>
        [Test]
        public void test_Parse_native_0_pt_0()
        {
            Assert.AreEqual("0/XRP", Amt("0.0").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_native_negative_0.
        /// </summary>
        [Test]
        public void test_Parse_native_negative_0()
        {
            Assert.AreEqual("0/XRP", Amt("-0").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_native_negative_0_pt_0.
        /// </summary>
        [Test]
        public void test_Parse_native_negative_0_pt_0()
        {
            Assert.AreEqual("0/XRP", Amt("-0.0").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_native_1000_drops.
        /// </summary>
        [Test]
        public void test_Parse_native_1000_drops()
        {
            Assert.AreEqual("0.001/XRP", Amt("1000").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_native_12_pt_3.
        /// </summary>
        [Test]
        public void test_Parse_native_12_pt_3()
        {
            Assert.AreEqual("12.3/XRP", Amt("12.3").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_native__12_pt_3.
        /// </summary>
        [Test]
        public void test_Parse_native__12_pt_3()
        {
            Assert.AreEqual("-12.3/XRP", Amt("-12.3").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_123_trailing_pt_ usd.
        /// </summary>
        [Test]
        public void test_Parse_123_trailing_pt_USD()
        {
            Assert.AreEqual(
                "123/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", 
                Amt("123./USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_12300_ usd.
        /// </summary>
        [Test]
        public void test_Parse_12300_USD()
        {
            Assert.AreEqual(
                "12300/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", 
                Amt("12300/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_12_pt_3_ usd.
        /// </summary>
        [Test]
        public void test_Parse_12_pt_3_USD()
        {
            Assert.AreEqual(
                "12.3/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", 
                Amt("12.3/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_1_pt_23_with_trailing_00_ usd.
        /// </summary>
        [Test]
        public void test_Parse_1_pt_23_with_trailing_00_USD()
        {
            Assert.AreEqual(
                "1.23/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", 
                Amt("1.2300/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh").ToTextFull());
        }

        /// <summary>
        /// The test_ parse_negative_0_ usd.
        /// </summary>
        [Test]
        public void test_Parse_negative_0_USD()
        {
            Assert.AreEqual(
                "0/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", 
                Amt("-0/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh").ToTextFull());
        }

        /// <summary>
        /// The test_ parse__0_pt_0_ usd.
        /// </summary>
        [Test]
        public void test_Parse__0_pt_0_USD()
        {
            Assert.AreEqual(
                "0/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh", 
                Amt("-0.0/USD/rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh").ToTextFull());
        }

        /// <summary>
        /// The assert offset.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        private void AssertOffset(string s, int i)
        {
            var Amt = Amount.FromString(s + "/USD/" + TestFixtures.BobAccount.Address);
            Assert.AreEqual(i, Amt.Offset, string.Format("Offset for {0} should be {1}", s, i));
        }

        /// <summary>
        /// The my decimal.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="BigDecimal"/>.
        /// </returns>
        private BigDecimal MyDecimal(string s)
        {
            return new BigDecimal(s);
        }

        /// <summary>
        /// The assert scale.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        private void AssertScale(string s, int i)
        {
            var bd = new BigDecimal(s);
            bd = bd.StripTrailingZeros();
            Assert.AreEqual(bd.Scale, i);
        }

        /// <summary>
        /// The Amt.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <returns>
        /// The <see cref="Amount"/>.
        /// </returns>
        private static Amount Amt(string s)
        {
            return Amount.FromString(s);
        }
    }
}