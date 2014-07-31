using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Fields;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx.Txns
{
    public class AccountSet : Transaction
    {
        public AccountSet()
            : base(Enums.TransactionType.AccountSet)
        {
        }

        public UInt32 TransferRate
        {
            get { return this[UInt32.TransferRate]; }
            set { Add(Field.TransferRate, value); }
        }

        public UInt32 WalletSize
        {
            get { return this[UInt32.WalletSize]; }
            set { Add(Field.WalletSize, value); }
        }

        public UInt32 SetFlag
        {
            get { return this[UInt32.SetFlag]; }
            set { Add(Field.SetFlag, value); }
        }

        public UInt32 ClearFlag
        {
            get { return this[UInt32.ClearFlag]; }
            set { Add(Field.ClearFlag, value); }
        }

        public Hash128 EmailHash
        {
            get { return this[Hash128.EmailHash]; }
            set { Add(Field.EmailHash, value); }
        }

        public Hash256 WalletLocator
        {
            get { return this[Hash256.WalletLocator]; }
            set { Add(Field.WalletLocator, value); }
        }

        public VariableLength MessageKey
        {
            get { return this[VariableLength.MessageKey]; }
            set { Add(Field.MessageKey, value); }
        }

        public VariableLength Domain
        {
            get { return this[VariableLength.Domain]; }
            set { Add(Field.Domain, value); }
        }
    }
}
