using System;
using System.Text;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Ripple.Core.Encodings.Base58
{
    public class B58
    {
        private int[] _mIndexes;
        private char[] _mAlphabet;

        public B58(string alphabet)
        {
            SetAlphabet(alphabet);
            BuildIndexes();
        }

        public string EncodeToStringChecked(byte[] input, int version)
        {
            return Encoding.GetEncoding("us-ascii").GetString(EncodeToBytesChecked(input, version));
        }

        public byte[] EncodeToBytesChecked(byte[] input, int version)
        {
            var buffer = new byte[input.Length + 1];
            buffer[0] = (byte)version;
            Array.Copy(input, 0, buffer, 1, input.Length);
            byte[] checkSum = CopyOfRange(Utils.Utils.DoubleDigest(buffer), 0, 4);
            var output = new byte[buffer.Length + checkSum.Length];
            Array.Copy(buffer, 0, output, 0, buffer.Length);
            Array.Copy(checkSum, 0, output, buffer.Length, checkSum.Length);
            return EncodeToBytes(output);
        }

        public string EncodeToString(byte[] input)
        {
            byte[] output = EncodeToBytes(input);
            return Encoding.GetEncoding("us-ascii").GetString(output);
        }

        /// <summary>
        /// Encodes the given bytes in base58. No checksum is appended.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] EncodeToBytes(byte[] input)
        {
            if (input.Length == 0)
            {
                return new byte[0];
            }

            input = CopyOfRange(input, 0, input.Length);

            // Count leading zeroes.
            int zeroCount = 0;

            while (zeroCount < input.Length && input[zeroCount] == 0)
            {
                ++zeroCount;
            }

            // The actual encoding.
            var temp = new byte[input.Length * 2];
            int j = temp.Length;

            int startAt = zeroCount;
            while (startAt < input.Length)
            {
                byte mod = Divmod58(input, startAt);
                if (input[startAt] == 0)
                {
                    ++startAt;
                }
                temp[--j] = (byte)_mAlphabet[mod];
            }

            // Strip extra '1' if there are some after decoding.
            while (j < temp.Length && temp[j] == _mAlphabet[0])
            {
                ++j;
            }

            // Add as many leading '1' as there were leading zeros.
            while (--zeroCount >= 0)
            {
                temp[--j] = (byte)_mAlphabet[0];
            }

            byte[] output = CopyOfRange(temp, j, temp.Length);
            return output;
        }

        public byte[] Decode(string input)
        {
            if (input.Length == 0)
            {
                return new byte[0];
            }

            var input58 = new byte[input.Length];

            // Transform the String to a base58 byte sequence
            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];

                int digit58 = -1;
                if (c >= 0 && c < 128)
                {
                    digit58 = _mIndexes[c];
                }
                if (digit58 < 0)
                {
                    throw new EncodingFormatException("Illegal character " + c + " at " + i);
                }

                input58[i] = (byte)digit58;
            }

            // Count leading zeroes
            int zeroCount = 0;
            while (zeroCount < input58.Length && input58[zeroCount] == 0)
            {
                ++zeroCount;
            }

            // The encoding
            var temp = new byte[input.Length];
            int j = temp.Length;

            int startAt = zeroCount;
            while (startAt < input58.Length)
            {
                byte mod = Divmod256(input58, startAt);
                if (input58[startAt] == 0)
                {
                    ++startAt;
                }

                temp[--j] = mod;
            }

            // Do no add extra leading zeroes, move j to first non null byte.
            while (j < temp.Length && temp[j] == 0)
            {
                ++j;
            }

            return CopyOfRange(temp, j - zeroCount, temp.Length);
        }

        public BigInteger DecodeToBigInteger(string input)
        {
            return new BigInteger(1, Decode(input));
        }

        /// <summary>
        /// Uses the checksum in the last 4 bytes of the decoded data to verify the rest are correct. The checksum is
        /// removed from the returned data.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version"></param>
        /// <exception cref="EncodingFormatException">
        /// If the input is not baseFields 58 or the checksum does not validate.
        /// </exception>
        /// <returns></returns>
        public byte[] DecodeCheched(string input, int version)
        {
            byte[] buffer = Decode(input);

            if (buffer.Length < 4)
                throw new EncodingFormatException("Input too short.");

            byte actualVersion = buffer[0];

            if (actualVersion != version)
            {
                throw new EncodingFormatException("Bro, version is wrong yo.");
            }

            byte[] toHash = CopyOfRange(buffer, 0, buffer.Length - 4);
            byte[] hashed = CopyOfRange(Utils.Utils.DoubleDigest(toHash), 0, 4);
            byte[] checksum = CopyOfRange(buffer, buffer.Length - 4, buffer.Length);

            if (!Arrays.AreEqual(checksum, hashed))
                throw new EncodingFormatException("Checksum does not validate.");

            return CopyOfRange(buffer, 1, buffer.Length - 4);
        }

        private void SetAlphabet(string alphabet)
        {
            _mAlphabet = alphabet.ToCharArray();
        }

        private void BuildIndexes()
        {
            _mIndexes = new int[128];

            for (int i = 0; i < _mIndexes.Length; i++)
            {
                _mIndexes[i] = -1;
            }

            for (int i = 0; i < _mAlphabet.Length; i++)
            {
                _mIndexes[_mAlphabet[i]] = i;
            }
        }

        /// <summary>
        /// number -> number / 58, returns number % 58
        /// </summary>
        /// <param name="number"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        private byte Divmod58(byte[] number, int startAt)
        {
            int remainder = 0;
            for (int i = startAt; i < number.Length; i++)
            {
                int digit256 = number[i] & 0xFF;
                int temp = remainder * 256 + digit256;

                number[i] = (byte)(temp / 58);

                remainder = temp % 58;
            }

            return (byte)remainder;
        }

        /// <summary>
        /// number -> number / 256, returns number % 256
        /// </summary>
        /// <param name="number58"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        private byte Divmod256(byte[] number58, int startAt)
        {
            int remainder = 0;
            for (int i = startAt; i < number58.Length; i++)
            {
                int digit58 = number58[i] & 0xFF;
                int temp = remainder * 58 + digit58;

                number58[i] = (byte)(temp / 256);

                remainder = temp % 256;
            }

            return (byte)remainder;
        }

        private byte[] CopyOfRange(byte[] source, int from, int to)
        {
            var range = new byte[to - from];
            Array.Copy(source, from, range, 0, range.Length);

            return range;
        }
    }
}
