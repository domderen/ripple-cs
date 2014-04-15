using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Types.Known.Sle.Entries
{
    public class DirectoryNode : LedgerEntry
    {
        public DirectoryNode()
            : base(Enums.LedgerEntryType.DirectoryNode)
        {
        }

        public Coretypes.UInt.UInt64 IndexNext
        {
            get { return base[Coretypes.UInt.UInt64.IndexNext]; }
            set { Add(Field.IndexNext, value); }
        }

        public Coretypes.UInt.UInt64 IndexPrevious
        {
            get { return base[Coretypes.UInt.UInt64.IndexPrevious]; }
            set { Add(Field.IndexPrevious, value); }
        }

        public Coretypes.UInt.UInt64 ExchangeRate
        {
            get { return base[Coretypes.UInt.UInt64.ExchangeRate]; }
            set { Add(Field.ExchangeRate, value); }
        }

        public Hash256 RootIndex
        {
            get { return base[Hash256.RootIndex]; }
            set { Add(Field.RootIndex, value); }
        }

        public AccountId Owner
        {
            get { return base[AccountId.Owner]; }
            set { Add(Field.Owner, value); }
        }

        public Hash160 TakerPaysCurrency
        {
            get { return base[Hash160.TakerPaysCurrency]; }
            set { Add(Field.TakerPaysCurrency, value); }
        }

        public Hash160 TakerPaysIssuer
        {
            get { return base[Hash160.TakerPaysIssuer]; }
            set { Add(Field.TakerPaysIssuer, value); }
        }

        public Hash160 TakerGetsCurrency
        {
            get { return base[Hash160.TakerGetsCurrency]; }
            set { Add(Field.TakerGetsCurrency, value); }
        }

        public Hash160 TakerGetsIssuer
        {
            get { return base[Hash160.TakerGetsIssuer]; }
            set { Add(Field.TakerGetsIssuer, value); }
        }

        public Vector256 Indexes
        {
            get { return base[Vector256.Indexes]; }
            set { Add(Field.Indexes, value); }
        }
    }
}
