using System;
using System.Collections.Generic;
using System.Text;

namespace Ripple.Core.Core.Serialized
{
    public class BytesList : IBytesSink
    {
        private readonly List<byte[]> _buffer  = new List<byte[]>();
        private int _len;

        static BytesList()
        {
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X").ToUpper();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                HexLookup[i] = s;
            }
        }

        public static string[] HexLookup = new string[256];

        public List<byte[]> RawList
        {
            get { return _buffer; }
        }

        public int BytesLength
        {
            get { return _len; }
        }

        public void Add(byte aByte)
        {
            Add(new []{aByte});
        }

        public void Add(byte[] bytes)
        {
            _len += bytes.Length;
            _buffer.Add(bytes);
        }

        public void Add(BytesList bl)
        {
            foreach (var bytes in bl.RawList)
            {
                Add(bytes);
            }
        }

        public byte[] Bytes()
        {
            int n = BytesLength;
            var bytes = new byte[n];
            AddBytes(bytes, 0);
            return bytes;
        }

        public string BytesHex()
        {
            var builder = new StringBuilder(_len * 2);
            foreach (var buf in _buffer)
            {
                foreach (var aBytes in buf)
                {
                    builder.Append(HexLookup[aBytes & 0xFF]);
                }
            }

            return builder.ToString();
        }

        // public void UpdateDigest(MessageDigest digest)
        // {
        //    foreach (var buf in _buffer)
        //    {
        //        digest.Update(buf);
        //    }
        // }

        private int AddBytes(byte[] bytes, int destPos)
        {
            foreach (var buf in _buffer)
            {
                Array.Copy(buf, 0, bytes, destPos, buf.Length);
                destPos += buf.Length;
            }
            return destPos;
        }
    }
}
