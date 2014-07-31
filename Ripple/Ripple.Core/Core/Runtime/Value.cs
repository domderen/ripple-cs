using System.Collections;
using Newtonsoft.Json.Linq;

namespace Ripple.Core.Core.Runtime
{
    using System;

    public enum Value
    {
        UNKNOWN,
        STRING,
        JSON_OBJECT,
        JSON_ARRAY,
        LIST,
        MAP,
        NUMBER,
        BYTE,
        DOUBLE,
        FLOAT,
        INTEGER,
        LONG,
        SHORT,
        BOOLEAN
    }

    public static class ValueUtils
    {
        public static Value TypeOf(object obj)
        {
            try
            {
                var jToken = (JToken)obj;

                if (jToken.Type == JTokenType.String)
                {
                    return Value.STRING;
                }

                if (jToken.Type == JTokenType.Bytes)
                {
                    return Value.BYTE;
                }

                if (jToken.Type == JTokenType.Float)
                {
                    return Value.FLOAT;
                }

                if (jToken.Type == JTokenType.Integer)
                {
                    return Value.INTEGER;
                }

                if (jToken.Type == JTokenType.Object)
                {
                    return Value.JSON_OBJECT;
                }

                if (jToken.Type == JTokenType.Array)
                {
                    return Value.JSON_ARRAY;
                }

                if (jToken.Type == JTokenType.Boolean)
                {
                    return Value.BOOLEAN;
                }
            }
            catch (Exception)
            {
            }

            if (obj is string)
            {
                return Value.STRING;
            }
            if (obj is byte)
            {
                return Value.BYTE;
            }
            if (obj is double)
            {
                return Value.DOUBLE;
            }
            if (obj is float)
            {
                return Value.FLOAT;
            }
            if (obj is int)
            {
                return Value.INTEGER;
            }
            if (obj is long)
            {
                return Value.LONG;
            }
            if (obj is short)
            {
                return Value.SHORT;
            }
            if (obj is JObject)
            {
                return Value.JSON_OBJECT;
            }
            if (obj is JArray)
            {
                return Value.JSON_ARRAY;
            }
            if (obj is DictionaryBase)
            {
                return Value.MAP;
            }
            if (obj is bool)
            {
                return Value.BOOLEAN;
            }
            if (obj is IList)
            {
                return Value.LIST;
            }
            return Value.UNKNOWN;
        }
    }
}
