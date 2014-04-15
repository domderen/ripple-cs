using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Types.Known.Tx.Result;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Sle
{
    /// <summary>
    /// This class has a PreviousTxnID and PreviousTxnLgrSeq.
    /// </summary>
    public abstract class ThreadedLedgerEntry : LedgerEntry
    {
        protected ThreadedLedgerEntry(LedgerEntryType type)
            : base(type)
        {
        }

        public void UpdateFromTransactionResult(TransactionResult tr)
        {
        }

        public UInt32 PreviousTxnLgrSeq
        {
            get { return this[UInt32.PreviousTxnLgrSeq]; }
            set { Add(Field.PreviousTxnLgrSeq, value); }
        }

        public Hash256 PreviousTxnID
        {
            get { return this[Hash256.PreviousTxnID]; }
            set { Add(Field.PreviousTxnID, value); }
        }
    }
}
