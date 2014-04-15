using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Fields;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx.Txns
{
    public class TrustSet : Transaction
    {
        public TrustSet()
            : base(Enums.TransactionType.TrustSet)
        {
        }

        public UInt32 QualityIn
        {
            get { return this[UInt32.QualityIn]; }
            set { Add(Field.QualityIn, value); }
        }

        public UInt32 QualityOut
        {
            get { return this[UInt32.QualityOut]; }
            set { Add(Field.QualityOut, value); }
        }

        public Amount LimitAmount
        {
            get { return this[Amount.LimitAmount]; }
            set { Add(Field.LimitAmount, value); }
        }
    }
}
