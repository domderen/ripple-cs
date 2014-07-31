using System;
using Ripple.Core.Core.Fields;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Core.Serialized
{
    /// <summary>
    /// This class should parse headers and object markers.
    /// TODO: Add capability for working with byte[] and offsets.
    /// </summary>
    public class BinaryParser
    {
        protected byte[] Bytes;
        private int _cursor;
        private readonly int _size;

        public BinaryParser(byte[] bytes)
        {
            _size = bytes.Length;
            Bytes = bytes;
        }
        public BinaryParser(string hex)
            : this(B16.Decode(hex))
        {
        }

        public int Size
        {
            get { return _size; }
        }

        public int Pos
        {
            get { return _cursor; }
        }

        /// <summary>
        /// Greater guard against infinite loops.
        /// </summary>
        public bool End
        {
            get { return _cursor >= Size; }
        }

        public Field ReadField()
        {
            int fieldCode = ReadFieldCode();
            var field = Field.FromCode(fieldCode);
            if (field == null)
            {
                throw new InvalidOperationException("Couldn't parse field from " + fieldCode.ToString("X"));
            }

            return field;
        }

        public int ReadFieldCode()
        {
            byte tagByte = ReadOne();

            uint typeBits = (uint)(tagByte & 0xFF) >> 4;
            if (typeBits == 0)
            {
                typeBits = ReadOne();
            }

            int fieldBits = tagByte & 0x0F;
            if (fieldBits == 0)
            {
                fieldBits = ReadOne();
            }

            return (int)(typeBits << 16 | fieldBits);
        }

        public int ReadVlLength()
        {
            int b1 = ReadOneInt();
            int result;

            if (b1 <= 192)
            {
                result = b1;
            }
            else if (b1 <= 240)
            {
                int b2 = ReadOneInt();
                result = 193 + (b1 - 193) * 256 + b2;
            }
            else if (b1 <= 254)
            {
                int b2 = ReadOneInt();
                int b3 = ReadOneInt();
                result = 12481 + (b1 - 141) * 65536 + b2 * 256 + b3;
            }
            else
            {
                throw new ApplicationException("Invalid varint length indicator.");
            }

            return result;
        }

        public byte[] Peek(int n)
        {
            return Read(n, false);
        }

        public byte PeekOne()
        {
            return Bytes[_cursor];
        }

        public byte[] Read(int n)
        {
            return Read(n, true);
        }

        public int ReadOneInt()
        {
            return ReadOne() & 0xFF;
        }

        public void Skip(int n)
        {
            _cursor += n;
        }

        public byte ReadOne()
        {
            return Bytes[_cursor++];
        }

        private byte[] Read(int n, bool advance)
        {
            var ret = new byte[n];
            Array.Copy(Bytes, _cursor, ret, 0, n);
            if (advance)
            {
                _cursor += n;
            }

            return ret;
        }
    }
}
