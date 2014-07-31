using System;
using Deveel.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;
using UInt64 = Ripple.Core.Core.Coretypes.UInt.UInt64;

namespace Ripple.Core.Core.Coretypes
{
    /// <summary>
    /// In ripple, amounts are either XRP, the native currency, or an IOU of
    /// a given currency as issued by a designated account.
    /// </summary>
    public class Amount : Number, ISerializedType, IComparable<Amount>
    {
        /// <summary>
        /// The maximum amount of digits in mantissa of an IOU amount.
        /// </summary>
        public const int MaximumIouPrecision = 16;

        /// <summary>
        /// The smallest quantity of an XRP is a drop, 1 millionth of an XRP.
        /// </summary>
        public const int MaximumNativeScale = 6;

        /// <summary>
        /// For rounding/Multiplying/dividing.
        /// </summary>
        public static MathContext MathContext = MathContext.Decimal64;

        // Defines bounds for native amounts.
        public static BigDecimal MaxNativeValue = ParseDecimal("100,000,000,000.0");
        public static BigDecimal MinNativeValue = ParseDecimal("0.000,001");

        // These are flags used when serializing to binary form.
        public static UInt64 BinaryFlagIsIou = new UInt64("8000000000000000", 16);
        public static UInt64 BinaryFlagIsNonNegativeNative = new UInt64("4000000000000000", 16);

        public static Amount OneXrp = FromString("1.0");

        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.AmountField AmountFld = new AmountField(Field.Amount);
        public static TypedFields.AmountField Balance = new AmountField(Field.Balance);
        public static TypedFields.AmountField LimitAmount = new AmountField(Field.LimitAmount);
        public static TypedFields.AmountField DeliveredAmount = new AmountField(Field.DeliveredAmount);
        public static TypedFields.AmountField TakerPays = new AmountField(Field.TakerPays);
        public static TypedFields.AmountField TakerGets = new AmountField(Field.TakerGets);
        public static TypedFields.AmountField LowLimit = new AmountField(Field.LowLimit);
        public static TypedFields.AmountField HighLimit = new AmountField(Field.HighLimit);
        public static TypedFields.AmountField Fee = new AmountField(Field.Fee);
        public static TypedFields.AmountField SendMax = new AmountField(Field.SendMax);
        public static TypedFields.AmountField MinimumOffer = new AmountField(Field.MinimumOffer);
        public static TypedFields.AmountField RippleEscrow = new AmountField(Field.RippleEscrow);

        /// <summary>
        /// The quantity of XRP or Issue(currency/issuer pairing)
        /// When native, the value unit is XRP, not drops.
        /// </summary>
        private BigDecimal _value;

        private readonly Currency _currency;

        /// <summary>
        /// If the currency is XRP.
        /// </summary>
        private readonly bool _isNative;

        /// <summary>
        /// Normally, in the constructor of an Amount the value is checked
        /// that it's scale/precision and quantity are correctly bounded.
        /// If unbounded is true, these checks are skipped.
        /// This is there for historical ledgers that contain amounts that
        /// would now be considered malformed (in the sense of the transaction 
        /// engine result class temMALFORMED).
        /// </summary>
        private readonly bool _unbounded;

        /// <summary>
        /// The ZERO account is used for specifying the issuer for native 
        /// amounts. In practice the issuer is never used when an
        /// amount is native.
        /// </summary>
        private AccountId _issuer;

        /// <summary>
        /// While internally the value is stored as a BigDecimal
        /// the mantissa and offset, as per the binary
        /// format can be computed.
        /// The mantissa is computed lazily, then cached.
        /// </summary>
        private UInt64 _mantissa;

        /// <summary>
        /// The offset is always calculated.
        /// </summary>
        private int _offset;

        public Amount(BigDecimal value, Currency currency, AccountId issuer)
            : this(value, currency, issuer, false)
        {
        }

        public Amount(BigDecimal value)
        {
            _isNative = true;
            _currency = Currency.Xrp;
            SetAndCheckValue(value);
        }

        public Amount(BigDecimal value, Currency currency, AccountId issuer, bool isNative, bool unbounded)
        {
            _isNative = isNative;
            _currency = currency;
            _unbounded = unbounded;
            SetAndCheckValue(value);
            // done AFTER set value which sets some default values
            _issuer = issuer;
        }

        public Amount(BigDecimal newValue, Currency currency, AccountId issuer, bool isNative)
            : this(newValue, currency, issuer, isNative, false)
        {
        }

        private Amount(BigDecimal value, string currency, string issuer)
            : this(value, currency)
        {
            if (issuer != null)
            {
                _issuer = AccountId.FromString(issuer);
            }
        }

        private Amount(BigDecimal value, string currency)
        {
            _isNative = false;
            _currency = Currency.FromString(currency);
            SetAndCheckValue(value);
        }

        public BigDecimal Value
        {
            get { return _value; }
        }

        public Currency Currency
        {
            get { return _currency; }
        }

        public AccountId Issuer
        {
            get { return _issuer; }
        }

        public bool IsNative
        {
            get { return _isNative; }
        }

        public int Offset
        {
            get { return _offset; }
        }

        public bool IsZero
        {
            get { return _value.Signum() == 0; }
        }

        public bool IsNegative
        {
            get { return _value.Signum() == -1; }
        }

        public string CurrencyString
        {
            get { return _currency.ToString(); }
        }

        public string IssuerString
        {
            get
            {
                if (_issuer == null)
                {
                    return "";
                }

                return _issuer.ToString();
            }
        }

        // Maybe you want !isNegative()
        // Any amount that !isNegative() isn't necessarily positive
        // Is a zero amount strictly positive? no
        private bool IsPositive
        {
            get { return _value.Signum() == 1; }
        }

        public static Amount FromString(string val)
        {
            if (val.Contains("/"))
            {
                return FromIouString(val);
            }

            if (val.Contains("."))
            {
                return FromXrpString(val);
            }

            return FromDropString(val);
        }

        public static Amount FromDropString(string val)
        {
            var drops = new BigDecimal(val).ScaleByPowerOfTen(-6);
            CheckDropsValueWhole(val);
            return new Amount(drops);
        }

        public static Amount FromIouString(string val)
        {
            var split = val.Split('/');
            if (split.Length == 1)
            {
                throw new ApplicationException("IOU string must be in the form number/currencyString or number/currencyString/issuerString.");
            }

            if (split.Length == 2)
            {
                return new Amount(new BigDecimal(split[0]), split[1]);
            }

            return new Amount(new BigDecimal(split[0]), split[1], split[2]);
        }

        public static void CheckXrpBounds(BigDecimal value)
        {
            value = value.Abs();
            CheckLowerDropBound(value);
            CheckUpperBound(value);
        }

        public static void CheckLowerDropBound(BigDecimal val)
        {
            if (val.Scale > 6)
            {
                throw GetOutOfBoundsException(val, "bigger", MinNativeValue);
            }
        }

        public static void CheckUpperBound(BigDecimal val)
        {
            if (val.CompareTo(MaxNativeValue) == 1)
            {
                throw GetOutOfBoundsException(val, "bigger", MaxNativeValue);
            }
        }

        public static void CheckDropsValueWhole(string drops)
        {
            bool contains = drops.Contains(".");

            if (contains)
            {
                throw new ApplicationException("Drops string contains floating point is decimal.");
            }
        }

        public static BigDecimal RoundValue(bool nativeSrc, BigDecimal value)
        {
            int i = value.Precision - value.Scale;

            return value.SetScale(nativeSrc ? MaximumNativeScale : MaximumIouPrecision - i, MathContext.RoundingMode);
        }

        public static BigDecimal XrpFromDropsMantissa(byte[] mantissa, int sign)
        {
            return new BigDecimal(new BigInteger(sign, mantissa), 6);
        }

        /**

        Arithimetic Operations

        There's no checking if an amount is of a different currency/issuer.
    
        All operations return amounts of the same currency/issuer as the
        first operand.

        eg.

            amountOne.Add(amountTwo)
            amountOne.Multiply(amountTwo)
            amountOne.Divide(amountTwo)
            amountOne.subtract(amountTwo)
        
            For all of these operations, the currency/issuer of the resultant
            amount, is that of `amountOne`
    
        Divide and Multiply are equivalent to the javascript ripple-lib
        ratio_human and product_huam.

        */

        public Amount Add(BigDecimal augend)
        {
            return NewValue(_value.Add(augend));
        }

        public Amount Add(Amount augend)
        {
            return Add(augend._value);
        }

        public Amount Add(Number augend)
        {
            return Add(BigDecimal.ValueOf(augend.DoubleValue()));
        }

        public Amount Subtract(BigDecimal subtrahend)
        {
            return NewValue(_value.Subtract(subtrahend));
        }

        public Amount Subtract(Amount subtrahend)
        {
            return this.Subtract(subtrahend._value);
        }

        public Amount Subtract(Number subtrahend)
        {
            return this.Subtract(BigDecimal.ValueOf(subtrahend.DoubleValue()));
        }

        public Amount Multiply(BigDecimal multiplicand)
        {
            return NewValue(_value.Multiply(multiplicand, MathContext), true);
        }

        public Amount Multiply(Amount multiplicand)
        {
            return Multiply(multiplicand._value);
        }

        public Amount Multiply(Number multiplicand)
        {
            return Multiply(BigDecimal.ValueOf(multiplicand.DoubleValue()));
        }

        public Amount Divide(BigDecimal divisor)
        {
            return NewValue(_value.Divide(divisor, MathContext), true);
        }

        public Amount Divide(Amount divisor)
        {
            return Divide(divisor._value);
        }

        public Amount Divide(Number divisor)
        {
            return Divide(BigDecimal.ValueOf(divisor.DoubleValue()));
        }

        public Amount Negate()
        {
            return NewValue(_value.Negate());
        }

        public Amount Abs()
        {
            return NewValue(_value.Abs());
        }

        public Amount Min(Amount val)
        {
            return (CompareTo(val) <= 0 ? this : val);
        }

        public Amount Max(Amount val)
        {
            return (CompareTo(val) >= 0 ? this : val);
        }

        public override bool Equals(object obj)
        {
            var a = obj as Amount;
            if (a != null)
            {
                return Equals(a);
            }

            return base.Equals(obj);
        }

        public bool Equals(Amount Amt)
        {
            return EqualValue(Amt) && _currency.Equals(Amt._currency) && (IsNative || _issuer.Equals(Amt._issuer));
        }

        public bool EqualsExceptIssuer(Amount Amt)
        {
            return EqualValue(Amt) && CurrencyString.Equals(Amt.CurrencyString);
        }

        public int CompareTo(Amount amount)
        {
            return _value.CompareTo(amount._value);
        }

        public Issue Issue()
        {
            // TODO: store the currency and issuer as an Issue.
            return new Issue(_currency, _issuer);
        }

        public UInt64 Mantissa()
        {
            return _mantissa ?? (_mantissa = CalculateMantissa());
        }

        /* Offer related helpers */
        public BigDecimal ComputeQuality(Amount toExchangeThisWith)
        {
            return _value.Divide(toExchangeThisWith._value, MathContext.Decimal128);
        }

        /// <summary>
        /// The real native unit is a drop, one million of which are an XRP.
        /// We want `one` unit at XRP scale (1e6 drops), or if it's an IOU,
        /// just `one`.
        /// </summary>
        /// <returns>
        /// <see cref="Amount"/>.
        /// </returns>
        public Amount One()
        {
            if (IsNative)
            {
                return OneXrp;
            }

            return Issue().Amount(1);
        }

        public object ToJson()
        {
            if (IsNative)
            {
                return ToDropsString();
            }

            return ToJsonObject();
        }

        public byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public void ToBytesSink(IBytesSink to)
        {
            UInt64 man = Mantissa();

            if (IsNative)
            {
                if (!IsNegative)
                {
                    man = man.Or(BinaryFlagIsNonNegativeNative);
                }
                to.Add(man.ToByteArray());
            }
            else
            {
                int offset = Offset;
                UInt64 packed;

                if (IsZero)
                {
                    packed = BinaryFlagIsIou;
                }
                else if (IsNegative)
                {
                    packed = man.Or(new UInt64(((long)512 + 0 + 97 + offset)).ShiftLeft(64 - 10));
                }
                else
                {
                    packed = man.Or(new UInt64(((long)512 + 256 + 97 + offset)).ShiftLeft(64 - 10));
                }

                to.Add(packed.ToByteArray());
                to.Add(_currency.Bytes);
                to.Add(_issuer.Bytes);
            }
        }

        public JObject ToJsonObject()
        {
            try
            {
                var outObj = new JObject
                {
                    {"currency", CurrencyString},
                    {"value", ValueText()},
                    {"issuer", IssuerString}
                };

                return outObj;
            }
            catch (JsonException e)
            {
                throw new ApplicationException("Json serialization failed.", e);
            }
        }

        public override int IntValue()
        {
            return _value.ToInt32Exact();
        }

        public override long LongValue()
        {
            return _value.ToInt64Exact();
        }

        public override float FloatValue()
        {
            return _value.ToSingle();
        }

        public override double DoubleValue()
        {
            return _value.ToDouble();
        }

        public BigInteger BigIntegerValue()
        {
            return _value.ToBigIntegerExact();
        }

        /// <summary>
        /// A String containing the value as a decimal number (in XRP scale).
        /// </summary>
        /// <returns></returns>
        public string ValueText()
        {
            return _value.Signum() == 0 ? "0" : _value.ToPlainString();
        }

        public string ToDropsString()
        {
            if (!IsNative)
            {
                throw new ApplicationException("Amount is not native.");
            }

            return BigIntegerDrops().ToString();
        }

        /// <summary>
        /// A String representation as used by ripple json format.
        /// </summary>
        /// <returns></returns>
        public string StringRepr()
        {
            if (IsNative)
            {
                return ToDropsString();
            }

            return IouTextFull();
        }

        public string IouText()
        {
            return string.Format("{0}/{1}", ValueText(), CurrencyString);
        }

        public string IouTextFull()
        {
            return string.Format("{0}/{1}/{2}", ValueText(), CurrencyString, IssuerString);
        }

        public string ToTextFull()
        {
            if (IsNative)
            {
                return NativeText();
            }

            return IouTextFull();
        }

        public string NativeText()
        {
            return string.Format("{0}/XRP", ValueText());
        }

        public override string ToString()
        {
            return ToTextFull();
        }

        public string ToText()
        {
            if (IsNative)
            {
                return NativeText();
            }

            return IouText();
        }

        protected int CalculateOffset()
        {
            return -MaximumIouPrecision + _value.Precision - _value.Scale;
        }

        [Obsolete]
        private static Amount FromXrpString(string valueString)
        {
            var val = new BigDecimal(valueString);
            return new Amount(val);
        }

        private static BigDecimal ParseDecimal(string s)
        {
            return new BigDecimal(s.Replace(",", string.Empty));
        }

        private static PrecisionException GetOutOfBoundsException(BigDecimal abs, string sized, BigDecimal bound)
        {
            return new PrecisionException(string.Format("{0} is {1} than bound {2}", abs.ToPlainString(), sized, bound));
        }

        private bool EqualValue(Amount Amt)
        {
            return CompareTo(Amt) == 0;
        }

        private void SetAndCheckValue(BigDecimal value)
        {
            _value = value.StripTrailingZeros();
            Initialize();
        }

        private void Initialize()
        {
            if (IsNative)
            {
                _issuer = AccountId.Zero;
                if (!_unbounded)
                {
                    CheckXrpBounds(_value);
                }
                
                // Offset is unused for natve amounts.
                _offset = -6; // compared to drops.
            }
            else
            {
                if (_value.Precision > MaximumIouPrecision && !_unbounded)
                {
                    throw new PrecisionException("Overflow Error!");
                }

                _issuer = AccountId.One;
                _offset = CalculateOffset();
            }
        }

        private Amount NewValue(BigDecimal newValue)
        {
            return NewValue(newValue, false, false);
        }

        private Amount NewValue(BigDecimal val, bool round)
        {
            return NewValue(val, round, false);
        }

        private Amount NewValue(BigDecimal newValue, bool round, bool unbounded)
        {
            if (round)
            {
                newValue = RoundValue(_isNative, newValue);
            }

            return new Amount(newValue, _currency, _issuer, _isNative, unbounded);
        }

        /* Offset & Mantissa Helpers */

        /// <summary>
        /// Return a positive value for the mantissa.
        /// </summary>
        /// <returns></returns>
        private UInt64 CalculateMantissa()
        {
            if (IsNative)
            {
                return new UInt64(BigIntegerDrops().Abs());
            }

            return new UInt64(BigIntegerIouMantissa());
        }

        private BigInteger BigIntegerIouMantissa()
        {
            return ExactBigIntegerScaledByPowerOfTen(-_offset).Abs();
        }

        private BigInteger BigIntegerDrops()
        {
            return ExactBigIntegerScaledByPowerOfTen(MaximumNativeScale);
        }

        private BigInteger ExactBigIntegerScaledByPowerOfTen(int n)
        {
            return _value.ScaleByPowerOfTen(n).ToBigIntegerExact();
        }

        private class AmountField : TypedFields.AmountField
        {
            private readonly Field _f;

            public AmountField(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }

        public class OutTranslator : OutTypeTranslator<Amount>
        {
            public override Amount FromString(string s)
            {
                return Amount.FromString(s);
            }

            public override Amount FromParser(BinaryParser parser, int? hint)
            {
                BigDecimal value;
                byte[] mantissa = parser.Read(8);
                byte b1 = mantissa[0], b2 = mantissa[1];

                bool isIou = (b1 & 0x80) != 0;
                bool isPositive = (b1 & 0x40) != 0;
                int sign = isPositive ? 1 : -1;

                if (isIou)
                {
                    mantissa[0] = 0;
                    Currency curr = Currency.OutTranslate.FromParser(parser);
                    AccountId issuer = AccountId.OutTranslate.FromParser(parser);
                    int offset = ((b1 & 0x3F) << 2) + ((b2 & 0xff) >> 6) - 97;
                    mantissa[1] &= 0x3F;

                    value = new BigDecimal(new BigInteger(sign, mantissa), -offset);
                    return new Amount(value, curr, issuer, false);
                }

                mantissa[0] &= 0x3F;
                value = XrpFromDropsMantissa(mantissa, sign);
                return new Amount(value);
            }

            public override Amount FromJObject(JObject jsonObject)
            {
                try
                {
                    string valueString = jsonObject.GetValue("value").ToString();
                    string issuerString = jsonObject.GetValue("issuer").ToString();
                    string currencyString = jsonObject.GetValue("currency").ToString();
                    return new Amount(new BigDecimal(valueString), currencyString, issuerString);
                }
                catch (JsonException e)
                {
                    throw new ApplicationException("Json serialization failed,", e);
                }
            }
        }

        public class InTranslator : InTypeTranslator<Amount>
        {
            public override string ToString(Amount obj)
            {
                return obj.StringRepr();
            }

            public override JObject ToJObject(Amount obj)
            {
                return obj.ToJsonObject();
            }
        }
    }
}
