using System;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Runtime;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Serialized
{
    public interface IOutTypeTranslator<out TOut>
        where TOut : ISerializedType
    {
        TOut FromValue(object obj);

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
        TOut FromParser(BinaryParser parser, int? hint);

        TOut FromParser(BinaryParser parser);

        TOut FromBytes(byte[] b);

        TOut FromHex(string hex);

        TOut FromJObject(JObject jsonObject);

        TOut FromJsonArray(JArray jsonArray);

        TOut FromBoolean(bool b);

        TOut FromLong(long l);

        TOut FromInteger(int i);

        TOut FromDouble(double d);

        TOut FromString(string s);
    }

    public interface IInTypeTranslator<in TIn>
        where TIn : ISerializedType
    {
        bool ToBoolean(TIn obj);

        long ToLong(TIn obj);

        int ToInteger(TIn obj);

        double ToDouble(TIn obj);

        string ToString(TIn obj);

        JObject ToJObject(TIn obj);

        JArray ToJArray(TIn obj);

        object ToJson(TIn obj);

        void ToBytesSink(TIn obj, IBytesSink to);

        byte[] ToBytes(TIn obj);

        string ToHex(TIn obj);
    }

    public abstract class OutTypeTranslator<TOut> : IOutTypeTranslator<TOut>
        where TOut : ISerializedType
    {
        public TOut FromValue(object obj)
        {
            switch (ValueUtils.TypeOf(obj))
            {
                case Value.STRING:

                    if (obj is JToken)
                    {
                        return this.FromString(obj.ToString());
                    }

                    return this.FromString((string)obj);

                case Value.DOUBLE:
                    return FromDouble((double)obj);
                case Value.INTEGER:

                    if (obj is JToken)
                    {
                        return this.FromInteger(((JToken)obj).ToObject<int>());
                    }

                    return FromInteger((int)obj);

                case Value.LONG:
                    return FromLong((long)obj);
                case Value.BOOLEAN:
                    return FromBoolean((bool)obj);
                case Value.JSON_ARRAY:
                    return FromJsonArray((JArray)obj);
                case Value.JSON_OBJECT:
                    return FromJObject((JObject)obj);
                default:
                    return (TOut)obj;
            }
        }

        public virtual TOut FromJObject(JObject jsonObject)
        {
            throw new NotSupportedException();
        }

        public virtual TOut FromJsonArray(JArray jsonArray)
        {
            throw new NotSupportedException();
        }

        public virtual TOut FromBoolean(bool b)
        {
            throw new NotSupportedException();
        }

        public virtual TOut FromLong(long l)
        {
            throw new NotSupportedException();
        }

        public virtual TOut FromInteger(int i)
        {
            throw new NotSupportedException();
        }

        public virtual TOut FromDouble(double d)
        {
            throw new NotSupportedException();
        }

        public virtual TOut FromString(string s)
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
        public abstract TOut FromParser(BinaryParser parser, int? hint);

        public TOut FromParser(BinaryParser parser)
        {
            return FromParser(parser, null);
        }

        public TOut FromBytes(byte[] b)
        {
            return FromParser(new BinaryParser(b));
        }

        public TOut FromHex(string hex)
        {
            return FromBytes(B16.Decode(hex));
        }
    }

    public abstract class InTypeTranslator<TIn> : IInTypeTranslator<TIn>
        where TIn : ISerializedType
    {
        public virtual bool ToBoolean(TIn obj)
        {
            throw new NotSupportedException();
        }

        public virtual long ToLong(TIn obj)
        {
            throw new NotSupportedException();
        }

        public virtual int ToInteger(TIn obj)
        {
            throw new NotSupportedException();
        }

        public virtual double ToDouble(TIn obj)
        {
            throw new NotSupportedException();
        }

        public virtual string ToString(TIn obj)
        {
            return obj.ToString();
        }

        public virtual JObject ToJObject(TIn obj)
        {
            throw new NotSupportedException();
        }

        public virtual JArray ToJArray(TIn obj)
        {
            throw new NotSupportedException();
        }

        public virtual object ToJson(TIn obj)
        {
            return obj.ToJson();
        }

        public virtual void ToBytesSink(TIn obj, IBytesSink to)
        {
            obj.ToBytesSink(to);
        }

        public byte[] ToBytes(TIn obj)
        {
            var to = new BytesList();
            ToBytesSink(obj, to);
            return to.Bytes();
        }

        public string ToHex(TIn obj)
        {
            var to = new BytesList();
            ToBytesSink(obj, to);
            return to.BytesHex();
        }
    }
}
