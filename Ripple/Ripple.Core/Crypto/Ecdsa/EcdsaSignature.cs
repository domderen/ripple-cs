using System;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Ripple.Core.Crypto.Ecdsa
{
    public class EcdsaSignature
    {
        /// <summary>
        /// Two components of the signature.
        /// </summary>
        public BigInteger R, S;

        public EcdsaSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }

        public static EcdsaSignature DecodeFromDer(byte[] bytes)
        {
            try
            {
                var decoder = new Asn1InputStream(bytes);
                var seq = (Asn1Sequence)decoder.ReadObject();
                DerInteger r, s;
                try
                {
                    r = (DerInteger)seq[0];
                    s = (DerInteger)seq[1];
                }
                catch (InvalidCastException)
                {
                    return null;
                }
                decoder.Close();

                // OpenSSL deviates from the DER spec by interpreting these values as unsigned, though they should not be
                // Thus, we always use the positive versions. See: http://r6.ca/blog/20111119T211504Z.html
                return new EcdsaSignature(r.PositiveValue, s.PositiveValue);
            }
            catch (IOException e)
            {
                throw new ApplicationException("Decoding form DER failed", e);
            }
        }

        /// <summary>
        /// DER is an international standard for serializing data structures which is widely used in cryptography.
        /// It's somewhat like protocol buffers but less convenient. This method returns a standard DER encoding
        /// of the signature, as recognized by OpenSSL and other libraries.
        /// </summary>
        /// <returns></returns>
        public byte[] EncodeToDer()
        {
            return DerByteStream().ToArray();
        }

        protected MemoryStream DerByteStream()
        {
            // Usually 70-72 bytes.
            var bos = new MemoryStream(72);
            var seq = new DerSequenceGenerator(bos);
            seq.AddObject(new DerInteger(R));
            seq.AddObject(new DerInteger(S));
            seq.Close();
            return bos;
        }
    }
}
