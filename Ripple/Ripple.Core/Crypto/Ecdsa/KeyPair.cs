using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Ripple.Core.Crypto.Ecdsa
{
    public class KeyPair : IKeyPair
    {
        private readonly BigInteger _priv;
        private readonly BigInteger _pub;

        private readonly byte[] _pubBytes;

        public KeyPair(BigInteger priv, BigInteger pub)
        {
            _priv = priv;
            _pub = pub;
            _pubBytes = pub.ToByteArray();
        }

        public static bool Verify(byte[] data, byte[] sigBytes, BigInteger pub)
        {
            EcdsaSignature signature = EcdsaSignature.DecodeFromDer(sigBytes);
            var signer = new ECDsaSigner();
            ECPoint pubPoint = Secp256K1.Curve().DecodePoint(pub.ToByteArray());
            var parameters = new ECPublicKeyParameters(pubPoint, Secp256K1.Params());
            signer.Init(false, parameters);
            try
            {
                return signer.VerifySignature(data, signature.R, signature.S);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static byte[] Sign(byte[] bytes, BigInteger secret)
        {
            EcdsaSignature sig = CreateEcdsaSignature(bytes, secret);
            byte[] der = sig.EncodeToDer();
            if (!IsStrictlyCanonical(der))
            {
                throw new InvalidOperationException("Signature is not strictly canonical.");
            }

            return der;
        }

        public static bool IsStrictlyCanonical(byte[] sig)
        {
            return CheckIsCanonical(sig, true);
        }

        public static bool CheckIsCanonical(byte[] sig, bool strict)
        {
            // Make sure signature is canonical
            // To protect against signature morphing attacks

            // Signature should be:
            // <30> <len> [ <02> <lenR> <R> ] [ <02> <lenS> <S> ]
            // where
            // 6 <= len <= 70
            // 1 <= lenR <= 33
            // 1 <= lenS <= 33

            int sigLen = sig.Length;

            if ((sigLen < 8) || (sigLen > 72))
                return false;

            if ((sig[0] != 0x30) || (sig[1] != (sigLen - 2)))
                return false;

            // Find R and check its length
            const int rPos = 4;
            int rLen = sig[rPos - 1];

            if ((rLen < 1) || (rLen > 33) || ((rLen + 7) > sigLen))
                return false;

            // Find S and check its length
            int sPos = rLen + 6, sLen = sig[sPos - 1];
            if ((sLen < 1) || (sLen > 33) || ((rLen + sLen + 6) != sigLen))
                return false;

            if ((sig[rPos - 2] != 0x02) || (sig[sPos - 2] != 0x02))
                return false; // R or S have wrong type

            if ((sig[rPos] & 0x80) != 0)
                return false; // R is negative

            if ((sig[rPos] == 0) && rLen == 1)
                return false; // R is zero

            if ((sig[rPos] == 0) && ((sig[rPos + 1] & 0x80) == 0))
                return false; // R is padded

            if ((sig[sPos] & 0x80) != 0)
                return false; // S is negative

            if ((sig[sPos] == 0) && sLen == 1)
                return false; // S is zero

            if ((sig[sPos] == 0) && ((sig[sPos + 1] & 0x80) == 0))
                return false; // S is padded

            var rBytes = new byte[rLen];
            var sBytes = new byte[sLen];

            Array.Copy(sig, rPos, rBytes, 0, rLen);
            Array.Copy(sig, sPos, sBytes, 0, sLen);

            BigInteger r = new BigInteger(1, rBytes), s = new BigInteger(1, sBytes);

            BigInteger order = Secp256K1.Order();

            if (r.CompareTo(order) != -1 || s.CompareTo(order) != -1)
            {
                return false; // R or S greater than modulus
            }

            if (strict)
            {
                return order.Subtract(s).CompareTo(s) != -1;
            }

            return true;
        }

        public byte[] Sign(byte[] bytes)
        {
            return Sign(bytes, _priv);
        }

        public bool Verify(byte[] data, byte[] sigBytes)
        {
            return Verify(data, sigBytes, _pub);
        }

        public BigInteger Pub()
        {
            return _pub;
        }

        public byte[] PubBytes()
        {
            return _pubBytes;
        }

        public BigInteger Priv()
        {
            return _priv;
        }

        public string PubHex()
        {
            return Utils.Utils.BigHex(_pub);
        }

        public string PrivHex()
        {
            return Utils.Utils.BigHex(_priv);
        }

        private static EcdsaSignature CreateEcdsaSignature(byte[] bytes, BigInteger secret)
        {
            var signer = new ECDsaSigner();
            var privKey = new ECPrivateKeyParameters(secret, Secp256K1.Params());
            signer.Init(true, privKey);
            BigInteger[] sigs = signer.GenerateSignature(bytes);
            BigInteger r = sigs[0], s = sigs[1];

            BigInteger otherS = Secp256K1.Order().Subtract(s);
            if (s.CompareTo(otherS) == 1)
            {
                s = otherS;
            }

            return new EcdsaSignature(r, s);
        }
    }
}
