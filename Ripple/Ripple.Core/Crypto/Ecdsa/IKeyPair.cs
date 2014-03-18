using Org.BouncyCastle.Math;

namespace Ripple.Core.Crypto.Ecdsa
{
    public interface IKeyPair
    {
        string PubHex();

        BigInteger Pub();

        byte[] PubBytes();

        string PrivHex();

        BigInteger Priv();

        bool Verify(byte[] data, byte[] sigBytes);

        byte[] Sign(byte[] bytes);
    }
}
