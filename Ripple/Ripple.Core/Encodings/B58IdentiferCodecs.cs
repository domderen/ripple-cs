using Ripple.Core.Encodings.Base58;

namespace Ripple.Core.Encodings
{
    public class B58IdentiferCodecs
    {
        public const int VER_ACCOUNT_ID = 0;
        public const int VER_FAMILY_SEED = 33;

        public const int VER_NONE = 1;
        public const int VER_NODE_PUBLIC = 28;
        public const int VER_NODE_PRIVATE = 32;
        public const int VER_ACCOUNT_PUBLIC = 35;
        public const int VER_ACCOUNT_PRIVATE = 34;
        public const int VER_FAMILY_GENERATOR = 41;

        private readonly B58 _b58;

        public B58IdentiferCodecs(B58 base58Encoder)
        {
            _b58 = base58Encoder;
        }

        public byte[] DecodeFamilySeed(string masterSeed)
        {
            return _b58.DecodeCheched(masterSeed, VER_FAMILY_SEED);
        }

        public string EncodeFamilySeed(byte[] bytes)
        {
            return _b58.EncodeToStringChecked(bytes, VER_FAMILY_SEED);
        }

        public string EncodeAddress(byte[] bytes)
        {
            return _b58.EncodeToStringChecked(bytes, VER_ACCOUNT_ID);
        }

        public byte[] DecodeAddress(string address)
        {
            return _b58.DecodeCheched(address, VER_ACCOUNT_ID);
        }
    }
}
