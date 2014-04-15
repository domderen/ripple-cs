using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Fields;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx.Txns
{
    public class Payment : Transaction
    {
        public Payment()
            : base(Enums.TransactionType.Payment)
        {
        }

        public UInt32 DestinationTag
        {
            get { return this[UInt32.DestinationTag]; }
            set { Add(Field.DestinationTag, value); }
        }

        public Hash256 InvoiceID
        {
            get { return this[Hash256.InvoiceID]; }
            set { Add(Field.InvoiceID, value); }
        }

        public Amount Amount
        {
            get { return this[Amount.AmountFld]; }
            set { Add(Field.Amount, value); }
        }

        public Amount SendMax
        {
            get { return this[Amount.SendMax]; }
            set { Add(Field.SendMax, value); }
        }

        public AccountId Destination
        {
            get { return this[AccountId.Destination]; }
            set { Add(Field.Destination, value); }
        }

        public PathSet Paths
        {
            get { return this[PathSet.Paths]; }
            set { Add(Field.Paths, value); }
        }
    }
}
