using Deveel.Math;
using Ripple.Core.Core.Coretypes.Hash;

namespace Ripple.Core.Core.Coretypes
{
    public static class Quality
    {
        public static BigDecimal FromBookDirectory(Hash256 bookDirectory, bool payIsNative, bool getIsNative)
        {
            byte[] value = bookDirectory.Slice(-7);
            int offset = bookDirectory.Get(-8) - 100;
            return new BigDecimal(new BigInteger(1, value), 
                -(payIsNative ? 
                    offset - 6 : 
                    getIsNative ? 
                        offset + 6 : 
                        offset));
        }

        public static BigDecimal FromOfferBookDirectory(StObject offer)
        {
            return FromBookDirectory(offer[Hash256.BookDirectory], offer[Amount.TakerPays].IsNative,
                offer[Amount.TakerPays].IsNative);
        }
    }
}
