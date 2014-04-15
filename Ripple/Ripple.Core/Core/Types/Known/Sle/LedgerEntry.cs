using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Formats;

namespace Ripple.Core.Core.Types.Known.Sle
{
    public class LedgerEntry : StObject
    {
        public LedgerEntry(LedgerEntryType type)
        {
            SetFormat(SleFormat.Formats[type]);
            Add(Coretypes.UInt.UInt16.LedgerEntryType, type.AsInteger);
        }

        public Hash256 Index
        {
            get { return base[Hash256.index]; }
        }

        public Coretypes.UInt.UInt32 Flags
        {
            get { return base[Coretypes.UInt.UInt32.Flags]; }
            set { Add(Field.Flags, value); }
        }

        public Hash256 LedgerIndex
        {
            get { return base[Hash256.LedgerIndex]; }
            set { Add(Field.LedgerIndex, value);}
        }

        public LedgerEntryType LedgerEntryType()
        {
            return LedgerEntryType(this);
        }

        public void LedgerEntryType(Coretypes.UInt.UInt16 val)
        {
            Add(Field.LedgerEntryType, val);
        }

        public void LedgerEntryType(LedgerEntryType val)
        {
            Add(Field.LedgerEntryType, val.AsInteger);
        }
    }
}
