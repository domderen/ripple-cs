using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes
{
    public class PathSet : List<PathSet.Path>, ISerializedType
    {
        public static byte PathSeparatorByte = 0xFF;
        public static byte PathsetEndByte = 0x00;

        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.PathSetField Paths = new PathSetField(Field.Paths);

        public object ToJson()
        {
            return ToJArray();
        }

        public JArray ToJArray()
        {
            var array = new JArray();

            foreach (var path in this)
            {
                array.Add(path.ToJArray());
            }

            return array;
        }

        public void ToBytesSink(IBytesSink to)
        {
            int n = 0;

            foreach (var path in this)
            {
                if (n++ != 0)
                {
                    to.Add(PathSeparatorByte);
                }

                foreach (var hop in path)
                {
                    int type = hop.Type;
                    to.Add((byte) type);

                    if (hop.Account != null)
                    {
                        to.Add(hop.Account.Bytes);
                    }

                    if (hop.Currency != null)
                    {
                        to.Add(hop.Currency.Bytes);
                    }

                    if (hop.Issuer != null)
                    {
                        to.Add(hop.Issuer.Bytes);
                    }
                }
            }

            to.Add(PathsetEndByte);
        }

        public string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public class Path : List<Hop>
        {
            public static Path FromJArray(JArray array)
            {
                var path = new Path();
                int nHops = array.Count;

                for (int i = 0; i < nHops; i++)
                {
                    try
                    {
                        var hop = array[i];
                        path.Add(Hop.FromJObject(hop.ToObject<JObject>()));
                    }
                    catch (JsonException e)
                    {
                        throw new ApplicationException("Json deserialization failed.", e);
                    }
                }

                return path;
            }

            public JArray ToJArray()
            {
                var array = new JArray();
                foreach (var hop in this)
                {
                    array.Add(hop.ToJObject());
                }

                return array;
            }
        }

        public class Hop
        {
            public static byte TypeAccount = 0x01;
            public static byte TypeCurrency = 0x10;
            public static byte TypeIssuer = 0x20;
            
            public AccountId Account;
            public AccountId Issuer;
            public Currency Currency;
            public int type;

            public static Hop FromJObject(JObject json)
            {
                var hop = new Hop();

                try
                {
                    JToken account;
                    if (json.TryGetValue("account", out account))
                    {
                        hop.Account = AccountId.FromAddress(account.ToObject<string>());
                    }

                    JToken issuer;
                    if (json.TryGetValue("issuer", out issuer))
                    {
                        hop.Issuer = AccountId.FromAddress(issuer.ToObject<string>());
                    }

                    JToken currency;
                    if (json.TryGetValue("currency", out currency))
                    {
                        hop.SetCurrency(currency.ToObject<string>());
                    }

                    JToken type;
                    if (json.TryGetValue("type", out type))
                    {
                        hop.type = type.ToObject<int>();
                    }
                }
                catch (JsonException e)
                {
                    throw new ApplicationException("Json deserialization failed.", e);
                }

                return hop;
            }

            public JObject ToJObject()
            {
                var obj = new JObject();
                try
                {
                    obj.Add("type", Type);

                    if (Account != null)
                    {
                        obj.Add("account", new JObject(AccountId.InTranslate.ToJson(Account)));
                    }

                    if (Issuer != null)
                    {
                        obj.Add("issuer", new JObject(AccountId.InTranslate.ToJson(Issuer)));
                    }

                    if (CurrencyString != null)
                    {
                        obj.Add("currency", CurrencyString);
                    }
                }
                catch (JsonException e)
                {
                    throw new ApplicationException("Json serialization failed.", e);
                }

                return obj;
            }

            public string CurrencyString
            {
                get
                {
                    if (Currency == null)
                    {
                        return null;
                    }

                    return Currency.ToString();
                }
            }

            public int Type
            {
                get
                {
                    if (type == 0)
                    {
                        SynthesizeType();
                    }

                    return type;
                }
            }

            public void SetCurrency(string currency)
            {
                Currency = Currency.OutTranslate.FromString(currency);
            }

            public void SetCurrency(byte[] read)
            {
                Currency = new Currency(read);
            }

            public void SynthesizeType()
            {
                type = 0;

                if (Account != null)
                {
                    type |= TypeAccount;
                }

                if (CurrencyString != null)
                {
                    type |= TypeCurrency;
                }

                if (Issuer != null)
                {
                    type |= TypeIssuer;
                }
            }
        }

        public class OutTranslator : OutTypeTranslator<PathSet>
        {
            public override PathSet FromParser(BinaryParser parser, int? hint)
            {
                var pathSet = new PathSet();
                Path path = null;

                while (!parser.End)
                {
                    byte type = parser.ReadOne();

                    if (type == PathsetEndByte)
                    {
                        break;
                    }

                    if (path == null)
                    {
                        path = new Path();
                        pathSet.Add(path);
                    }

                    if (type == PathSeparatorByte)
                    {
                        path = null;
                        continue;
                    }

                    var hop = new Hop();
                    path.Add(hop);

                    if ((type & Hop.TypeAccount) != 0)
                    {
                        hop.Account = AccountId.OutTranslate.FromParser(parser);
                    }

                    if ((type & Hop.TypeCurrency) != 0)
                    {
                        hop.Currency = Currency.OutTranslate.FromParser(parser);
                    }

                    if ((type & Hop.TypeIssuer) != 0)
                    {
                        hop.Issuer = AccountId.OutTranslate.FromParser(parser);
                    }
                }

                return pathSet;
            }

            public override PathSet FromJsonArray(JArray jsonArray)
            {
                var paths = new PathSet();

                int nPaths = jsonArray.Count;

                for (int i = 0; i < nPaths; i++)
                {
                    try
                    {
                        var path = jsonArray[i].ToObject<JArray>();
                        paths.Add(Path.FromJArray(path));
                    }
                    catch (JsonException e)
                    {
                        throw new ApplicationException("Json deserialization failed.", e);
                    }
                }

                return paths;
            }
        }

        public class InTranslator : InTypeTranslator<PathSet>
        {
        }

        private class PathSetField : TypedFields.PathSetField
        {
            private readonly Field _f;

            public PathSetField(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }
    }
}
