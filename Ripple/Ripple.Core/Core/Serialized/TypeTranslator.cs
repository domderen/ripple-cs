using System;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Runtime;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Serialized
{
    public abstract class TypeTranslator<T> where T : ISerializedType
    {
        public T FromValue(object obj)
        {
            switch (ValueUtils.TypeOf(obj))
            {
                case Value.STRING:
                    return FromString((string)obj);
                case Value.DOUBLE:
                    return FromDouble((double)obj);
                case Value.INTEGER:
                    return FromInteger((int)obj);
                case Value.LONG:
                    return FromLong((long)obj);
                case Value.BOOLEAN:
                    return FromBoolean((bool)obj);
                case Value.JSON_ARRAY:
                    return FromJsonArray((JArray)obj);
                case Value.JSON_OBJECT:
                    return FromJsonObject((JObject)obj);
                default:
                    return (T)obj;
            }
        }

        public virtual bool ToBoolean(T obj)
        {
            throw new NotSupportedException();
        }

        public virtual long ToLong(T obj)
        {
            throw new NotSupportedException();
        }

        public virtual int ToInteger(T obj)
        {
            throw new NotSupportedException();
        }

        public virtual double ToDouble(T obj)
        {
            throw new NotSupportedException();
        }

        public virtual string ToString(T obj)
        {
            return obj.ToString();
        }

        public T FromJsonObject(JObject jsonObject)
        {
            throw new NotSupportedException();
        }

        public T FromJsonArray(JArray jsonArray)
        {
            throw new NotSupportedException();
        }

        public T FromBoolean(bool b)
        {
            throw new NotSupportedException();
        }

        public T FromLong(long l)
        {
            throw new NotSupportedException();
        }

        public T FromInteger(int i)
        {
            throw new NotSupportedException();
        }

        public T FromDouble(double d)
        {
            throw new NotSupportedException();
        }

        public virtual T FromString(string s)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="hint">
        /// Using a boxed integer, allowing null for no hint.
        /// This generic parameter can be used to hint the amount
        /// of bytes (VL) (or for any other purpose desired).
        /// </param>
        /// <returns></returns>
        public abstract T FromParser(BinaryParser parser, int? hint);

        public T FromParser(BinaryParser parser)
        {
            return FromParser(parser, null);
        }

        public T FromBytes(byte[] b)
        {
            return FromParser(new BinaryParser(b));
        }

        public T FromHex(string hex)
        {
            return FromBytes(B16.Decode(hex));
        }

        public JObject ToJObject(T obj)
        {
            throw new NotSupportedException();
        }

        public JArray ToJArray(T obj)
        {
            throw new NotSupportedException();
        }

        public virtual object ToJson(T obj)
        {
            return obj.ToJson();
        }

        public virtual void ToBytesSink(T obj, IBytesSink to)
        {
            obj.ToBytesSink(to);
        }

        public byte[] ToBytes(T obj)
        {
            var to = new BytesList();
            ToBytesSink(obj, to);
            return to.Bytes();
        }

        public string ToHex(T obj)
        {
            var to = new BytesList();
            ToBytesSink(obj, to);
            return to.BytesHex();
        }
    }
}
