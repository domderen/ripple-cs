using System;
using System.Text.RegularExpressions;
using Deveel.Math;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Coretypes
{
    /// <summary>
    /// Funnily enough, yes, in rippled a currency is represented by a Hash160 type.
    /// For the sake of consistency and convenience, this quirk is repeated here.
    /// 
    /// https://gist.github.com/justmoon/8597643
    /// </summary>
    public class Currency : Hash<Currency>
    {
        // This is used to represent a native currency.
        public static byte[] Zero = new byte[20];
        public static Currency Xrp = new Currency(Zero);

        public static OutCurrencyTranslator OutTranslate = new OutCurrencyTranslator();
        public static InCurrencyTranslator InTranslate = new InCurrencyTranslator();
        public Demurrage demurrage = null;
        private readonly Type _type;

        public Currency(byte[] bytes)
            : base(bytes, 20)
        {
            _type = TypeHelper.FromBytes(bytes[0]);

            if (_type == Type.DEMURRAGE)
            {
                demurrage = new Demurrage(bytes);
            }
        }

        public static Currency FromString(string currency)
        {
            return OutTranslate.FromString(currency);
        }

        /// <summary>
        /// The following are static methods, legacy from when there was no
        /// usage of Currency objects, just String with "XRP" ambiguity.
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        public static byte[] EncodeCurrency(string currencyCode)
        {
            var currencyBytes = new byte[20];
            currencyBytes[12] = (byte) currencyCode[0];
            currencyBytes[13] = (byte)currencyCode[1];
            currencyBytes[14] = (byte)currencyCode[2];

            return currencyBytes;
        }

        public static string GetCurrencyCodeFromTlcBytes(byte[] bytes)
        {
            int i;
            bool zeroInNonCurrencyBytes = true;

            for (i = 0; i < 20; i++)
            {
                zeroInNonCurrencyBytes = zeroInNonCurrencyBytes &&
                    ((i == 12 || i == 13 || i == 14) || // currency bytes (0 or any other).
                    bytes[i] == 0); // non currency bytes (0).
            }

            if (zeroInNonCurrencyBytes)
            {
                return IsoCodeFromBytesAndOffset(bytes, 12);
            }

            throw new InvalidOperationException("Currency is invalid.");
        }

        private static char CharFrom(byte[] bytes, int i)
        {
            return (char) bytes[i];
        }

        private static string IsoCodeFromBytesAndOffset(byte[] bytes, int offset)
        {
            var a = CharFrom(bytes, offset);
            var b = CharFrom(bytes, offset + 1);
            var c = CharFrom(bytes, offset + 2);

            return "" + a + b + c;
        }

        public string HumanCode()
        {
            if (_type == Type.ISO)
            {
                return GetCurrencyCodeFromTlcBytes(HashBytes);
            }

            if (_type == Type.DEMURRAGE)
            {
                return IsoCodeFromBytesAndOffset(HashBytes, 1);
            }

            throw new InvalidOperationException(string.Format("No human code for currency of type {0}.", _type));
        }

        public override object ToJson()
        {
            return InTranslate.ToJson(this);
        }

        public override byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public override string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public override void ToBytesSink(IBytesSink to)
        {
            InTranslate.ToBytesSink(this, to);
        }

        public override string ToString()
        {
            switch (_type)
            {
                case Type.ISO:
                    var code = GetCurrencyCodeFromTlcBytes(Bytes);
                    if (code == "XRP")
                    {
                        // HEX of the bytes.
                        return base.ToString();
                    }

                    if (code == "\0\0\0")
                    {
                        return "XRP";
                    }

                    // The 3 letter isoCode.
                    return code;
                default:
                    return base.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            var currency = obj as Currency;
            if (currency != null)
            {
                byte[] bytes = Bytes;
                byte[] otherBytes = currency.Bytes;

                if (_type == Type.ISO && currency._type == Type.ISO)
                {
                    return (bytes[12] == otherBytes[12] && bytes[13] == otherBytes[13] && bytes[13] == otherBytes[13]);
                }
            }
            return base.Equals(obj);
        }

        public enum Type
        {
            HASH,
            ISO, // Three letter isoCode.
            DEMURRAGE,
            UNKNOWN
        }

        public static class TypeHelper
        {
            public static Type FromBytes(byte typeByte)
            {
                if (typeByte == 0x00)
                {
                    return Type.ISO;
                }

                if (typeByte == 0x01)
                {
                    return Type.DEMURRAGE;
                }

                if ((typeByte & 0x80) != 0)
                {
                    return Type.HASH;
                }

                return Type.UNKNOWN;
            }
        }

        public class Demurrage
        {
            private readonly RippleDate _intrestStart;

            private readonly string _isoCode;

            private readonly double _intrestRate;

            public Demurrage(byte[] bytes)
            {
                var parser = new BinaryParser(bytes);
                parser.Skip(1); // The type.
                _isoCode = IsoCodeFromBytesAndOffset(parser.Read(3), 0);
                _intrestStart = RippleDate.FromParser(parser);
                long l = UInt.UInt64.OutTranslate.FromParser(parser).LongValue();
                _intrestRate = BitConverter.Int64BitsToDouble(l);
            }

            public string IsoCode
            {
                get { return _isoCode; }
            }

            public double IntrestRate
            {
                get { return _intrestRate; }
            }

            public RippleDate IntrestStart
            {
                get { return _intrestStart; }
            }

            public static BigDecimal ApplyRate(BigDecimal amount, BigDecimal rate, TimeSpan timeSpan)
            {
                BigDecimal appliedRate = GetSeconds(timeSpan).Divide(rate, MathContext.Decimal64);
                BigDecimal factor = BigDecimal.ValueOf(Math.Exp(appliedRate.ToDouble()));
                return amount.Multiply(factor, MathContext.Decimal64);
            }

            public static BigDecimal CalculateRate(BigDecimal rate, TimeSpan timeSpan)
            {
                BigDecimal seconds = GetSeconds(timeSpan);
                BigDecimal log = Ln(rate);
                return seconds.Divide(log, MathContext.Decimal64);
            }

            private static BigDecimal Ln(BigDecimal bd)
            {
                return BigDecimal.ValueOf(Math.Log(bd.ToDouble()));
            }

            private static BigDecimal GetSeconds(TimeSpan timeSpan)
            {
                return BigDecimal.ValueOf(timeSpan.TotalSeconds);
            }
        }

        public class OutCurrencyTranslator : OutHashTranslator<Currency>
        {
            public override int ByteWidth()
            {
                return 20;
            }

            public override Currency NewInstance(byte[] b)
            {
                return new Currency(b);
            }

            public override Currency FromString(string s)
            {
                if (s.Length == 40)
                {
                    return NewInstance(B16.Decode(s));
                }

                if (s == "XRP")
                {
                    return Xrp;
                }

                if (!Regex.IsMatch(s, "[A-Z0-9]{3}"))
                {
                    throw new ApplicationException("Currency code must be 3 characters.");
                }

                return NewInstance(EncodeCurrency(s));
            }
        }

        public class InCurrencyTranslator : InHashTranslator<Currency>
        {
        }
    }
}
