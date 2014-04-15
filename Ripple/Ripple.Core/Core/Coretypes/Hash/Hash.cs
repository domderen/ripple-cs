using System;
using Deveel.Math;
using Org.BouncyCastle.Utilities;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Coretypes.Hash
{
    public abstract class Hash<TSubclass> : ISerializedType, IComparable<TSubclass> where TSubclass : Hash<TSubclass>
    {
        protected readonly byte[] HashBytes;
        protected int HashCode = -1;

        protected Hash(byte[] bytes, int size)
        {
            HashBytes = NormalizeAndChechHash(bytes, size);
        }

        public byte[] Bytes
        {
            get { return HashBytes; }
        }

        public override string ToString()
        {
            return B16.ToString(HashBytes);
        }

        public override bool Equals(object obj)
        {
            if (obj is Hash<TSubclass>)
            {
                return Arrays.AreEqual(HashBytes, ((Hash<TSubclass>)obj).HashBytes);
            }

            return base.Equals(obj);
        }

        public int CompareTo(TSubclass another)
        {
            int thisLength = Bytes.Length;
            byte[] bytes = another.Bytes;

            for (int i = 0; i < thisLength; i++)
            {
                int cmp = HashBytes[i] - bytes[i];
                if (cmp != 0)
                {
                    return cmp;
                }
            }

            return 0;
        }

        public byte[] Slice(int start)
        {
            return Slice(start, 0);
        }

        public byte[] Slice(int start, int end)
        {
            if (start < 0) start += HashBytes.Length;
            if (end <= 0) end += HashBytes.Length;

            int length = end - start;
            var slice = new byte[length];

            Array.Copy(HashBytes, start, slice, 0, length);
            return slice;
        }

        public byte Get(int i)
        {
            if (i < 0) i += HashBytes.Length;
            return HashBytes[i];
        }

        public override int GetHashCode()
        {
            if (HashCode == -1)
            {
                HashCode = new BigInteger(1, HashBytes).GetHashCode();
            }

            return HashCode;
        }

        protected BigInteger BigInteger()
        {
            return new BigInteger(1, HashBytes);
        }

        private byte[] NormalizeAndChechHash(byte[] bytes, int size)
        {
            int length = bytes.Length;
            if (length > size)
            {
                const string simpleName = "";

                throw new ApplicationException(string.Format("Hash length of {0} is too wide for {1}.", length, simpleName));
            }

            if (length == size)
            {
                return bytes;
            }

            var hash = new byte[size];
            Array.Copy(bytes, 0, hash, size - length, length);
            return hash;
        }

        public abstract class HashTranslator<T> : TypeTranslator<T> where T : Hash<T>
        {
            public abstract T NewInstance(byte[] b);
            public abstract int ByteWidth();

            public override T FromParser(BinaryParser parser, int? hint)
            {
                return NewInstance(parser.Read(ByteWidth()));
            }

            public override object ToJson(T obj)
            {
                return B16.ToString(obj.HashBytes);
            }

            public override T FromString(string s)
            {
                return NewInstance(B16.Decode(s));
            }

            public override void ToBytesSink(T obj, IBytesSink to)
            {
                to.Add(obj.HashBytes);
            }
        }

        public abstract object ToJson();
        public abstract byte[] ToBytes();
        public abstract string ToHex();
        public abstract void ToBytesSink(IBytesSink to);
    }
}
