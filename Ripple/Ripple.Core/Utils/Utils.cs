using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Core.Encodings.Common;

namespace Ripple.Core.Utils
{
    public static class Utils
    {
        private static HashAlgorithm _digest = new SHA256CryptoServiceProvider();

        /// <summary>
        /// <see cref="DoubleDigest(byte[], int, int)"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] DoubleDigest(byte[] input)
        {
            return DoubleDigest(input, 0, input.Length);
        }

        /// <summary>
        /// Calculates the SHA-256 hash of the given byte range, and then hashes the resulting hash again. This is
        /// standard procedure in Bitcoin. The resulting hash is in big endian form.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static byte[] DoubleDigest(byte[] input, int offset, int length)
        {
            _digest.Dispose();
            _digest = new SHA256CryptoServiceProvider();
            byte[] first = _digest.ComputeHash(input, offset, length);
            return _digest.ComputeHash(first);
        }

        public static BigInteger HexBig(string hex)
        {
            return new BigInteger(1, Hex.Decode(hex));
        }

        public static byte[] HalfSha512(byte[] bytes)
        {
            var hash = new byte[32];
            Array.Copy(Sha512(bytes), 0, hash, 0, 32);
            return hash;
        }

        public static byte[] QuarterSha512(byte[] bytes)
        {
            var hash = new byte[16];
            Array.Copy(Sha512(bytes), 0, hash, 0, 16);
            return hash;
        }

        public static byte[] Sha512(byte[] byteArrays)
        {
            HashAlgorithm messageDigest = new SHA512CryptoServiceProvider();
            messageDigest.TransformFinalBlock(byteArrays, 0, byteArrays.Length);
            return messageDigest.Hash;
        }

        public static byte[] SHA256_RIPEMD160(byte[] input)
        {
            byte[] sha256 = new SHA256CryptoServiceProvider().ComputeHash(input);
            var digest = new RipeMD160Digest();
            digest.BlockUpdate(sha256, 0, sha256.Length);
            var outArray = new byte[20];
            digest.DoFinal(outArray, 0);
            return outArray;
        }

        public static string BigHex(BigInteger bn)
        {
            return B16.ToString(bn.ToByteArray());
        }

        public static BigInteger UBigInt(byte[] bytes)
        {
            return new BigInteger(1, bytes);
        }
    }
}
