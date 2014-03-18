using System;
using Org.BouncyCastle.Utilities;
using Ripple.Core.Core.Fields;
using Type = System.Type;

namespace Ripple.Core.Core.Serialized
{
    public class BinarySerializer
    {
        private readonly IBytesSink _sink;

        public BinarySerializer(IBytesSink sink)
        {
            _sink = sink;
        }

        public static byte[] EncodeVl(int length)
        {
            var lenBytes = new byte[4];

            if (length <= 192)
            {
                lenBytes[0] = (byte)(length);
                return Arrays.Copy(lenBytes, 0, 1);
            }

            if (length <= 12480)
            {
                length -= 193;
                lenBytes[0] = (byte)(193 + (((uint)length) >> 8));
                lenBytes[1] = (byte)(length & 0xff);
                return Arrays.Copy(lenBytes, 0, 2);
            }

            if (length <= 918744)
            {
                length -= 12481;
                lenBytes[0] = (byte)(241 + (((uint)length) >> 16));
                lenBytes[1] = (byte)((length >> 8) & 0xff);
                lenBytes[2] = (byte)(length & 0xff);
                return Arrays.Copy(lenBytes, 0, 3);
            }

            throw new ApplicationException("Overflow error.");
        }

        public void Add(byte[] n)
        {
            _sink.Add(n);
        }

        public void AddLengthEncoded(byte[] n)
        {
            Add(EncodeVl(n.Length));
            Add(n);
        }

        public void Add(BytesList bl)
        {
            foreach (var bytes in bl.RawList)
            {
                _sink.Add(bytes);
            }
        }

        public int AddFieldHeader(Field f)
        {
            if (!f.IsSerialized())
            {
                throw new InvalidOperationException(string.Format("Field {0} is a discardable field", f));
            }

            byte[] n = f.Bytes;
            Add(n);
            return n.Length;
        }

        public void Add(byte type)
        {
            _sink.Add(type);
        }

        public void AddLengthEncoded(BytesList bytes)
        {
            Add(EncodeVl(bytes.BytesLength));
            Add(bytes);
        }

        public void Add(Field field, ISerializedType value)
        {
            AddFieldHeader(field);
            if (field.IsVlEncoded())
            {
                AddLengthEncoded(value);
            }
            else
            {
                value.ToBytesSink(_sink);
                if (field.Type == Fields.Type.StObject)
                {
                    AddFieldHeader(Field.ObjectEndMarker);
                }
                else if (field.Type == Fields.Type.StArray)
                {
                    AddFieldHeader(Field.ArrayEndMarker);
                }
            }
        }

        public void AddLengthEncoded(ISerializedType value)
        {
            var bytes = new BytesList();
            value.ToBytesSink(bytes);
            AddLengthEncoded(bytes);
        }
    }
}
