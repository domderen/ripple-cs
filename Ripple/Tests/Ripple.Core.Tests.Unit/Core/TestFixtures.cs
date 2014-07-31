namespace Ripple.Core.Tests.Unit.Core
{
    using Ripple.Core.Core.Coretypes;

    public static class TestFixtures
    {
        /// <summary>
        /// From wallet_propose masterpassphrase.
        /// </summary>
        public const string MasterSeedAddress = "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh";

        public const string MasterSeed = "snoPBrXtMeMyMHUVTgbuqAfg1SUTb";

        public static byte[] MasterSeedBytes =
            {
                0xde, 0xdc, 0xe9, 0xce, 0x67, 0xb4, 0x51, 0xd8, 0x52, 0xfd, 0x4e,
                0x84, 0x6f, 0xcd, 0xe3, 0x1c
            };

        public const string SingedMasterSeedBytes = "3046022100eb46f96961453219b5e2baa263f01d66c3c7d3ca0672623f13b9a24508d0a56c022100b1b64510c7f415e902f00071d821a112fe489f0fb4fb6f87b411e88a73b40e70";

        public static AccountId RootAccount = AccountId.FromSeed(MasterSeed);

        public static AccountId BobAccount = AccountId.FromSeed("shn6zJ8zzkaoFSfsEWvJLZf3V344C");
    }
}
