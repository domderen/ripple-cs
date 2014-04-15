using Ripple.Core.Encodings;
using Ripple.Core.Encodings.Base58;

namespace Ripple.Core.Config
{
    public class Config
    {
        public const string DefaultAlphabet = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";

        private static B58IdentiferCodecs _b58IdentiferCodecs;

        public static bool BouncyInitiated = false;

        static Config()
        {
            SetAlphabet(DefaultAlphabet);
            FeeCushion = 1.1;
            InitBouncy();
        }

        public static B58IdentiferCodecs B58IdentiferCodecs
        {
            get { return _b58IdentiferCodecs; }
        }

        public static double FeeCushion { get; set; }

        public static void SetAlphabet(string alphabet)
        {
            var b58 = new B58(alphabet);
            _b58IdentiferCodecs = new B58IdentiferCodecs(b58);
        }

        public static void InitBouncy()
        {
            if (!BouncyInitiated)
            {
                // Security.addProvider(new BouncyCastleProvider());
                BouncyInitiated = true;
            }
        }
    }
}
