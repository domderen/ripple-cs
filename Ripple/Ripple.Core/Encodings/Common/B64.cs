using Org.BouncyCastle.Utilities.Encoders;

namespace Ripple.Core.Encodings.Common
{
    public static class B64
    {
        public static string ToString(byte[] bytes)
        {
            return Base64.ToBase64String(bytes);
        }

        public static byte[] Decode(string str)
        {
            return Base64.Decode(str);
        }
    }
}
