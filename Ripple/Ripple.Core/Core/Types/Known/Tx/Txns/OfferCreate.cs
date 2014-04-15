using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Fields;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx.Txns
{
    public class OfferCreate : Transaction
    {
        public OfferCreate()
            : base(Enums.TransactionType.OfferCreate)
        {
        }

        public UInt32 Expiration
        {
            get { return this[UInt32.Expiration]; }
            set { Add(Field.Expiration, value); }
        }

        public UInt32 OfferSequence
        {
            get { return this[UInt32.OfferSequence]; }
            set { Add(Field.OfferSequence, value); }
        }

        public Amount TakerPays
        {
            get { return this[Amount.TakerPays]; }
            set { Add(Field.TakerPays, value); }
        }

        public Amount TakerGets
        {
            get { return this[Amount.TakerGets]; }
            set { Add(Field.TakerGets, value); }
        }
    }
}
