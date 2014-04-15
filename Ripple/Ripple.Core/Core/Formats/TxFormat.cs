using System;
using System.Collections.Generic;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Formats
{
    public sealed class TxFormat : Format
    {
        public TxFormat(TransactionType type, params Object[] args)
            : base(args)
        {
            TransactionType = type;
            AddCommonFields();
            Formats.Add(type, this);
        }

        public static IDictionary<TransactionType, TxFormat> Formats = new Dictionary<TransactionType, TxFormat>();

        public readonly TransactionType TransactionType;

        public static TxFormat AccountSet = new TxFormat(
            TransactionType.AccountSet,
            Field.EmailHash, Requirement.OPTIONAL,
            Field.WalletLocator, Requirement.OPTIONAL,
            Field.WalletSize, Requirement.OPTIONAL,
            Field.MessageKey, Requirement.OPTIONAL,
            Field.Domain, Requirement.OPTIONAL,
            Field.TransferRate, Requirement.OPTIONAL,
            Field.SetFlag, Requirement.OPTIONAL,
            Field.ClearFlag, Requirement.OPTIONAL);

        public static TxFormat TrustSet = new TxFormat(
            TransactionType.TrustSet,
            Field.LimitAmount, Requirement.OPTIONAL,
            Field.QualityIn, Requirement.OPTIONAL,
            Field.QualityOut, Requirement.OPTIONAL);

        public static TxFormat OfferCreate = new TxFormat(
            TransactionType.OfferCreate,
            Field.TakerPays, Requirement.REQUIRED,
            Field.TakerGets, Requirement.REQUIRED,
            Field.Expiration, Requirement.OPTIONAL,
            Field.OfferSequence, Requirement.OPTIONAL);

        public static TxFormat OfferCancel = new TxFormat(
            TransactionType.OfferCancel,
            Field.OfferSequence, Requirement.REQUIRED);

        public static TxFormat SetRegularKey = new TxFormat(
            TransactionType.SetRegularKey,
            Field.RegularKey, Requirement.OPTIONAL);

        public static TxFormat Payment = new TxFormat(
            TransactionType.Payment,
            Field.Destination, Requirement.REQUIRED,
            Field.Amount, Requirement.REQUIRED,
            Field.SendMax, Requirement.OPTIONAL,
            Field.Paths, Requirement.DEFAULT,
            Field.InvoiceID, Requirement.OPTIONAL,
            Field.DestinationTag, Requirement.OPTIONAL);

        public static TxFormat Contract = new TxFormat(
            TransactionType.Contract,
            Field.Expiration, Requirement.REQUIRED,
            Field.BondAmount, Requirement.REQUIRED,
            Field.StampEscrow, Requirement.REQUIRED,
            Field.RippleEscrow, Requirement.REQUIRED,
            Field.CreateCode, Requirement.OPTIONAL,
            Field.FundCode, Requirement.OPTIONAL,
            Field.RemoveCode, Requirement.OPTIONAL,
            Field.ExpireCode, Requirement.OPTIONAL);

        public static TxFormat RemoveContract = new TxFormat(
            TransactionType.RemoveContract,
            Field.Target, Requirement.REQUIRED);

        public static TxFormat EnableFeature = new TxFormat(
            TransactionType.EnableFeature,
            Field.Feature, Requirement.REQUIRED);

        public static TxFormat SetFee = new TxFormat(
            TransactionType.SetFee,
            Field.BaseFee, Requirement.REQUIRED,
            Field.ReferenceFeeUnits, Requirement.REQUIRED,
            Field.ReserveBase, Requirement.REQUIRED,
            Field.ReserveIncrement, Requirement.REQUIRED);

        public static TxFormat FromString(string name)
        {
            return GetTxFormat(TransactionType.FromString(name));
        }

        public static TxFormat FromNumber(Number ord)
        {
            return GetTxFormat(TransactionType.FromNumber(ord));
        }

        public static TxFormat FromValue(Object o)
        {
            if (o is int)
            {
                return FromNumber((Number) o);
            }

            var s = o as string;
            if (s != null)
            {
                return FromString(s);
            }

            return null;
        }

        public static TxFormat GetTxFormat(TransactionType key)
        {
            if (key == null) return null;
            return Formats[key];
        }

        public override void AddCommonFields()
        {
            Put(Field.TransactionType, Requirement.REQUIRED);
            Put(Field.Account, Requirement.REQUIRED);
            Put(Field.Sequence, Requirement.REQUIRED);
            Put(Field.Fee, Requirement.REQUIRED);
            Put(Field.SigningPubKey, Requirement.REQUIRED);

            Put(Field.Flags, Requirement.OPTIONAL);
            Put(Field.SourceTag, Requirement.OPTIONAL);
            Put(Field.PreviousTxnID, Requirement.OPTIONAL);
            Put(Field.OperationLimit, Requirement.OPTIONAL);
            Put(Field.TxnSignature, Requirement.OPTIONAL);
            Put(Field.AccountTxnID, Requirement.OPTIONAL);
            Put(Field.LastLedgerSequence, Requirement.OPTIONAL);
        }
    }
}
