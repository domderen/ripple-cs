using System;
using Deveel.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Coretypes.UInt;

namespace Ripple.Core.Core.Coretypes
{
    /// <summary>
    /// Represents a currency/issuer pair.
    /// </summary>
    public class Issue
    {
        public static Issue Xrp = FromString("XRP");

        private readonly Currency _currency;
        private readonly AccountId _issuer;

        public Issue(Currency currency, AccountId issuer)
        {
            _currency = currency;
            _issuer = issuer;
        }

        public Currency Currency
        {
            get { return _currency; }
        }

        public AccountId Issuer
        {
            get { return _issuer; }
        }

        private bool IsNative
        {
            get { return this == Xrp || _currency.Equals(Currency.Xrp); }
        }

        public static Issue FromString(string pair)
        {
            var split = pair.Split('/');
            return GetIssue(split);
        }

        public override string ToString()
        {
            if (IsNative)
            {
                return "XRP";
            }

            return string.Format("{0}/{1}", _currency, _issuer);
        }

        public Amount Amount(BigDecimal value)
        {
            return new Amount(value, _currency, _issuer, IsNative);
        }

        public Amount Amount(Number value)
        {
            return new Amount(BigDecimal.ValueOf(value.LongValue()), _currency, _issuer, IsNative);    
        }

        public JObject ToJson()
        {
            var o = new JObject();
            try
            {
                o.Add("currency", new JValue(_currency));
                o.Add("issuer", new JValue(_issuer));
            }
            catch (JsonException e)
            {
                throw new ApplicationException("Json creation failed.", e);
            }

            return o;
        }

        private static Issue GetIssue(string[] split)
        {
            if (split.Length == 2)
            {
                return new Issue(Currency.FromString(split[0]), AccountId.FromString(split[2]));
            }

            if (split[0] == "XRP")
            {
                return new Issue(Currency.Xrp, AccountId.Zero);
            }

            throw new ApplicationException("Issue string must be XRP or $currency/$issuer");
        }
    }
}
