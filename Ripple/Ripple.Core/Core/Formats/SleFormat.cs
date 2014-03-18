using System;
using System.Collections.Generic;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Formats
{
    public sealed class SleFormat : Format
    {
        public SleFormat(LedgerEntryType type, params Object[] args)
            : base(args)
        {
            LedgerEntryType = type;
            AddCommonFields();
            Formats.Add(type, this);
        }

        public static IDictionary<LedgerEntryType, SleFormat> Formats = new Dictionary<LedgerEntryType, SleFormat>();

        public static SleFormat AccountRoot = new SleFormat(
            LedgerEntryType.AccountRoot,
            Field.Account, Requirement.REQUIRED,
            Field.Sequence, Requirement.REQUIRED,
            Field.Balance, Requirement.REQUIRED,
            Field.OwnerCount, Requirement.REQUIRED,
            Field.PreviousTxnID, Requirement.REQUIRED,
            Field.PreviousTxnLgrSeq, Requirement.REQUIRED,
            Field.RegularKey, Requirement.OPTIONAL,
            Field.EmailHash, Requirement.OPTIONAL,
            Field.WalletLocator, Requirement.OPTIONAL,
            Field.WalletSize, Requirement.OPTIONAL,
            Field.MessageKey, Requirement.OPTIONAL,
            Field.TransferRate, Requirement.OPTIONAL,
            Field.Domain, Requirement.OPTIONAL
            );

        public static SleFormat Contract = new SleFormat(
            LedgerEntryType.Contract,
            Field.Account, Requirement.REQUIRED,
            Field.Balance, Requirement.REQUIRED,
            Field.PreviousTxnID, Requirement.REQUIRED,
            Field.PreviousTxnLgrSeq, Requirement.REQUIRED,
            Field.Issuer, Requirement.REQUIRED,
            Field.Owner, Requirement.REQUIRED,
            Field.Expiration, Requirement.REQUIRED,
            Field.BondAmount, Requirement.REQUIRED,
            Field.CreateCode, Requirement.OPTIONAL,
            Field.FundCode, Requirement.OPTIONAL,
            Field.RemoveCode, Requirement.OPTIONAL,
            Field.ExpireCode, Requirement.OPTIONAL
            );

        public static SleFormat DirectoryNode = new SleFormat(
            LedgerEntryType.DirectoryNode,
            Field.Owner, Requirement.OPTIONAL, // for owner directories
            Field.TakerPaysCurrency, Requirement.OPTIONAL, // for order book directories
            Field.TakerPaysIssuer, Requirement.OPTIONAL, // for order book directories
            Field.TakerGetsCurrency, Requirement.OPTIONAL, // for order book directories
            Field.TakerGetsIssuer, Requirement.OPTIONAL, // for order book directories
            Field.ExchangeRate, Requirement.OPTIONAL, // for order book directories
            Field.Indexes, Requirement.REQUIRED,
            Field.RootIndex, Requirement.REQUIRED,
            Field.IndexNext, Requirement.OPTIONAL,
            Field.IndexPrevious, Requirement.OPTIONAL
            );

        public static SleFormat GeneratorMap = new SleFormat(
            LedgerEntryType.GeneratorMap,
            Field.Generator, Requirement.REQUIRED
            );

        public static SleFormat Nickname = new SleFormat(
            LedgerEntryType.Nickname,
            Field.Account, Requirement.REQUIRED,
            Field.MinimumOffer, Requirement.OPTIONAL
            );

        public static SleFormat Offer = new SleFormat(
            LedgerEntryType.Offer,
            Field.Account, Requirement.REQUIRED,
            Field.Sequence, Requirement.REQUIRED,
            Field.TakerPays, Requirement.REQUIRED,
            Field.TakerGets, Requirement.REQUIRED,
            Field.BookDirectory, Requirement.REQUIRED,
            Field.BookNode, Requirement.REQUIRED,
            Field.OwnerNode, Requirement.REQUIRED,
            Field.PreviousTxnID, Requirement.REQUIRED,
            Field.PreviousTxnLgrSeq, Requirement.REQUIRED,
            Field.Expiration, Requirement.OPTIONAL
            );

        public static SleFormat RippleState = new SleFormat(
            LedgerEntryType.RippleState,
            Field.Balance, Requirement.REQUIRED,
            Field.LowLimit, Requirement.REQUIRED,
            Field.HighLimit, Requirement.REQUIRED,
            Field.PreviousTxnID, Requirement.REQUIRED,
            Field.PreviousTxnLgrSeq, Requirement.REQUIRED,
            Field.LowNode, Requirement.OPTIONAL,
            Field.LowQualityIn, Requirement.OPTIONAL,
            Field.LowQualityOut, Requirement.OPTIONAL,
            Field.HighNode, Requirement.OPTIONAL,
            Field.HighQualityIn, Requirement.OPTIONAL,
            Field.HighQualityOut, Requirement.OPTIONAL
            );

        public static SleFormat LedgerHashes = new SleFormat(
            LedgerEntryType.LedgerHashes,
            Field.FirstLedgerSequence, Requirement.OPTIONAL, // Remove if we do a ledger restart
            Field.LastLedgerSequence, Requirement.OPTIONAL,
            Field.Hashes, Requirement.REQUIRED
            );

        public static SleFormat EnabledFeatures = new SleFormat(
            LedgerEntryType.EnabledFeatures,
            Field.Features, Requirement.REQUIRED
            );

        public static SleFormat FeeSettings = new SleFormat(
            LedgerEntryType.FeeSettings,
            Field.BaseFee, Requirement.REQUIRED,
            Field.ReferenceFeeUnits, Requirement.REQUIRED,
            Field.ReserveBase, Requirement.REQUIRED,
            Field.ReserveIncrement, Requirement.REQUIRED
            );

        public readonly LedgerEntryType LedgerEntryType;

        public static SleFormat FromString(string name)
        {
            return GetLedgerFormat(LedgerEntryType.FromString(name));
        }

        public static SleFormat FromNumber(int ord)
        {
            return GetLedgerFormat(LedgerEntryType.FromNumber(ord));
        }

        public static SleFormat FromValue(Object o)
        {
            if (o is int)
            {
                return FromNumber((int)o);
            }

            var s = o as string;
            if (s != null)
            {
                return FromString(s);
            }

            return null;
        }

        public static SleFormat GetLedgerFormat(LedgerEntryType key)
        {
            if (key == null) return null;
            return Formats[key];
        }

        public override void AddCommonFields()
        {
            Put(Field.LedgerIndex, Requirement.OPTIONAL);
            Put(Field.LedgerEntryType, Requirement.REQUIRED);
            Put(Field.Flags, Requirement.REQUIRED);
        }
    }
}
