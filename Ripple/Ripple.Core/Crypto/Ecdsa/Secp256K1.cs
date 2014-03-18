using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Ripple.Core.Crypto.Ecdsa
{
    public class Secp256K1
    {
        private static readonly ECDomainParameters EcParams;

        static Secp256K1()
        {
            X9ECParameters parameters = SecNamedCurves.GetByName("secp256k1");
            EcParams = new ECDomainParameters(parameters.Curve, parameters.G, parameters.N, parameters.H);
        }

        public static ECDomainParameters Params()
        {
            return EcParams;
        }

        public static BigInteger Order()
        {
            return EcParams.N;
        }

        public static ECCurve Curve()
        {
            return EcParams.Curve;
        }

        public static ECPoint BasePoint()
        {
            return EcParams.G;
        }

        internal static byte[] BasePointMultipliedBy(BigInteger secret)
        {
            return BasePoint().Multiply(secret).GetEncoded(true);
        }
    }
}
