using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Types.Known.Sle.Entries
{
    public class AccountRoot : ThreadedLedgerEntry
    {
        public AccountRoot()
            : base(Enums.LedgerEntryType.AccountRoot)
        {
        }

        public Coretypes.UInt.UInt32 Sequence
        {
            get { return base[Coretypes.UInt.UInt32.Sequence]; }
            set { Add(Field.Sequence, value); }
        }

        public Coretypes.UInt.UInt32 TransferRate
        {
            get { return base[Coretypes.UInt.UInt32.TransferRate]; }
            set { Add(Field.TransferRate, value); }
        }

        public Coretypes.UInt.UInt32 WalletSize
        {
            get { return base[Coretypes.UInt.UInt32.WalletSize]; }
            set { Add(Field.WalletSize, value); }
        }

        public Coretypes.UInt.UInt32 OwnerCount
        {
            get { return base[Coretypes.UInt.UInt32.OwnerCount]; }
            set { Add(Field.OwnerCount, value); }
        }

        public Hash128 EmailHash
        {
            get { return base[Hash128.EmailHash]; }
            set { Add(Field.EmailHash, value); }
        }

        public Hash256 WalletLocator
        {
            get { return base[Hash256.WalletLocator]; }
            set { Add(Field.WalletLocator, value); }
        }

        public Amount Balance
        {
            get { return base[Amount.Balance]; }
            set { Add(Field.Balance, value); }
        }

        public VariableLength MessageKey
        {
            get { return base[VariableLength.MessageKey]; }
            set { Add(Field.MessageKey, value); }
        }

        public VariableLength Domain
        {
            get { return base[VariableLength.Domain]; }
            set { Add(Field.Domain, value); }
        }

        public AccountId Account
        {
            get { return base[AccountId.Account]; }
            set { Add(Field.Account, value); }
        }

        public AccountId RegularKey
        {
            get { return base[AccountId.RegularKey]; }
            set { Add(Field.RegularKey, value); }
        }
    }
}
