using System;
using System.Text;
using Org.BouncyCastle.Math;

namespace Ripple.Core.Crypto.Ecdsa
{
    public class Seed
    {
        public static byte[] PassPhraseToSeedBytes(string seed)
        {
            return Utils.Utils.QuarterSha512(Encoding.UTF8.GetBytes(seed));
        }

        public static IKeyPair CreateKeyPair(byte[] seedBytes)
        {
            BigInteger secret, pub, privateGen, order = Secp256K1.Order();
            byte[] privateGetBytes;
            byte[] publicGetBytes;

            int i = 0, seq = 0;

            while (true)
            {
                privateGetBytes = HashedIncrement(seedBytes, i++);
                privateGen = Utils.Utils.UBigInt(privateGetBytes);
                if (privateGen.CompareTo(order) == -1)
                {
                    break;
                }
            }

            publicGetBytes = Secp256K1.BasePointMultipliedBy(privateGen);

            i = 0;
            while (true)
            {
                byte[] secretBytes = HashedIncrement(AppendIntBytes(publicGetBytes, seq), i++);
                secret = Utils.Utils.UBigInt(secretBytes);
                if (secret.CompareTo(order) == -1)
                {
                    break;
                }
            }

            secret = secret.Add(privateGen).Mod(order);
            pub = Utils.Utils.UBigInt(Secp256K1.BasePointMultipliedBy(secret));

            return new KeyPair(secret, pub);
        }

        public static byte[] HashedIncrement(byte[] bytes, int increment)
        {
            return Utils.Utils.HalfSha512(AppendIntBytes(bytes, increment));
        }

        public static byte[] AppendIntBytes(byte[] inArray, long i)
        {
            var outArray = new byte[inArray.Length + 4];

            Array.Copy(inArray, 0, outArray, 0, inArray.Length);

            outArray[inArray.Length] = (byte)((((uint)i) >> 24) & 0xFF);
            outArray[inArray.Length + 1] = (byte)((((uint)i) >> 16) & 0xFF);
            outArray[inArray.Length + 2] = (byte)((((uint)i) >> 8) & 0xFF);
            outArray[inArray.Length + 3] = (byte)(((uint)i) & 0xFF);

            return outArray;
        }
    }
}
