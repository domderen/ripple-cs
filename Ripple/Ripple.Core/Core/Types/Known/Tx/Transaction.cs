using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Coretypes.Hash.Prefixes;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Formats;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx
{
    public class Transaction : StObject
    {
        public const bool CanonicalFlagDeployed = false;
        public static UInt32 CanonicalSignature = new UInt32(0x80000000L);

        public Transaction(TransactionType type)
        {
            SetFormat(TxFormat.Formats[type]);
            Add(UInt16.TransactionType, type.AsInteger);
        }

        public TransactionType TransactionType()
        {
            return TransactionType(this);
        }

        public Hash256 SigningHash()
        {
            Hash256.HalfSha512 signing = Hash256.Prefixed256(HashPrefix.TxSign);
            ToBytesSink(signing, new IsSigningFieldFilter());

            return signing.Finish();
        }

        public void SetCanonicalSignatureFlag()
        {
            var flags = base[UInt32.Flags];
            if (flags == null)
            {
                flags = CanonicalSignature;
            }
            else
            {
                flags.Or(CanonicalSignature);
            }

            Add(UInt32.Flags, flags);
        }

        public UInt32 Flags
        {
            get { return base[UInt32.Flags]; }
            set { Add(Field.Flags, value); }
        }

        public UInt32 SourceTag
        {
            get { return base[UInt32.SourceTag]; }
            set { Add(Field.SourceTag, value); }
        }

        public UInt32 Sequence
        {
            get { return base[UInt32.Sequence]; }
            set { Add(Field.Sequence, value); }
        }

        public UInt32 LastLedgerSequence
        {
            get { return base[UInt32.LastLedgerSequence]; }
            set { Add(Field.LastLedgerSequence, value); }
        }

        public UInt32 OperationLimit
        {
            get { return base[UInt32.OperationLimit]; }
            set { Add(Field.OperationLimit, value); }
        }

        public Hash256 PreviousTxnId
        {
            get { return base[Hash256.PreviousTxnID]; }
            set { Add(Field.PreviousTxnID, value); }
        }

        public Hash256 AccountTxnId
        {
            get { return base[Hash256.AccountTxnID]; }
            set { Add(Field.AccountTxnID, value); }
        }

        public Amount Fee
        {
            get { return base[Amount.Fee]; }
            set { Add(Field.Fee, value); }
        }

        public VariableLength SigningPubKey
        {
            get { return base[VariableLength.SigningPubKey]; }
            set { Add(Field.SigningPubKey, value); }
        }

        public VariableLength TxnSignature
        {
            get { return base[VariableLength.TxnSignature]; }
            set { Add(Field.TxnSignature, value); }
        }

        public AccountId Account
        {
            get { return base[AccountId.Account]; }
            set { Add(Field.Account, value); }
        }

        public void TransactionType(UInt16 val)
        {
            Add(Field.TransactionType, val);
        }

        private class IsSigningFieldFilter : IFieldFilter
        {
            public bool Evaluate(Field a)
            {
                return a.IsSigningField();
            }
        }
    }
}
