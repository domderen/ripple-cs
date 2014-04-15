using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Types.Known.Sle.Entries
{
    public class RippleState : ThreadedLedgerEntry
    {
        public RippleState()
            : base(Enums.LedgerEntryType.RippleState)
        {
        }

        public Coretypes.UInt.UInt32 HighQualityIn
        {
            get { return base[Coretypes.UInt.UInt32.HighQualityIn]; }
            set { Add(Field.HighQualityIn, value); }
        }

        public Coretypes.UInt.UInt32 HighQualityOut
        {
            get { return base[Coretypes.UInt.UInt32.HighQualityOut]; }
            set { Add(Field.HighQualityOut, value); }
        }

        public Coretypes.UInt.UInt32 LowQualityIn
        {
            get { return base[Coretypes.UInt.UInt32.LowQualityIn]; }
            set { Add(Field.LowQualityIn, value); }
        }

        public Coretypes.UInt.UInt32 LowQualityOut
        {
            get { return base[Coretypes.UInt.UInt32.LowQualityOut]; }
            set { Add(Field.LowQualityOut, value); }
        }

        public Coretypes.UInt.UInt64 LowNode
        {
            get { return base[Coretypes.UInt.UInt64.LowNode]; }
            set { Add(Field.LowNode, value); }
        }

        public Coretypes.UInt.UInt64 HighNode
        {
            get { return base[Coretypes.UInt.UInt64.HighNode]; }
            set { Add(Field.HighNode, value); }
        }

        public Amount Balance
        {
            get { return base[Amount.Balance]; }
            set { Add(Field.Balance, value); }
        }

        public Amount LowLimit
        {
            get { return base[Amount.LowLimit]; }
            set { Add(Field.LowLimit, value); }
        }

        public Amount HighLimit
        {
            get { return base[Amount.HighLimit]; }
            set { Add(Field.HighLimit, value); }
        }
    }
}
