using System.Collections.Generic;
using Deveel.Math;
using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Fields;

namespace Ripple.Core.Core.Types.Known.Sle.Entries
{
    public class Offer : ThreadedLedgerEntry
    {
        public static Comparer<Offer>  QualityAscending = new OfferComparator();

        public Offer()
            : base(Enums.LedgerEntryType.Offer)
        {
        }

        public Coretypes.UInt.UInt32 Sequence
        {
            get { return base[Coretypes.UInt.UInt32.Sequence]; }
            set { Add(Field.Sequence, value); }
        }

        public Coretypes.UInt.UInt32 Expiration
        {
            get { return base[Coretypes.UInt.UInt32.Expiration]; }
            set { Add(Field.Expiration, value); }
        }

        public Coretypes.UInt.UInt64 BookNode
        {
            get { return base[Coretypes.UInt.UInt64.BookNode]; }
            set { Add(Field.BookNode, value); }
        }

        public Coretypes.UInt.UInt64 OwnerNode
        {
            get { return base[Coretypes.UInt.UInt64.OwnerNode]; }
            set { Add(Field.OwnerNode, value); }
        }

        public Hash256 BookDirectory
        {
            get { return base[Hash256.BookDirectory]; }
            set { Add(Field.BookDirectory, value); }
        }

        public Amount TakerPays
        {
            get { return base[Amount.TakerPays]; }
            set { Add(Field.TakerPays, value); }
        }

        public Amount TakerGets
        {
            get { return base[Amount.TakerGets]; }
            set { Add(Field.TakerGets, value); }
        }

        public AccountId Account
        {
            get { return base[AccountId.Account]; }
            set { Add(Field.Account, value); }
        }

        public BigDecimal DirectoryAskQuality()
        {
            return Quality.FromOfferBookDirectory(this);
        }

        // TODO: these methods would be useful on an OfferCreate transaction too.
        public BigDecimal AskQuality
        {
            get { return TakerPays.ComputeQuality(TakerGets); }
        }

        public BigDecimal BigQuality
        {
            get { return TakerGets.ComputeQuality(TakerPays); }
        }

        public Amount GetsOne
        {
            get { return TakerGets.One(); }
        }

        public Amount PaysOne
        {
            get { return TakerPays.One(); }
        }

        public string GetPayCurrencyPair()
        {
            return TakerGets.CurrencyString + "/" + TakerPays.CurrencyString;
        }

        public StObject Executed(StObject finalFields)
        {
            // Where 'this' is an AffectedNode nodeAsPrevious.
            var executed = new StObject
            {
                {Amount.TakerPays, finalFields[Amount.TakerPays].Subtract(TakerPays).Abs()},
                {Amount.TakerGets, finalFields[Amount.TakerGets].Subtract(TakerGets).Abs()}
            };

            return executed;
        }

        public class OfferComparator : Comparer<Offer>
        {
            public override int Compare(Offer x, Offer y)
            {
                return x.DirectoryAskQuality().CompareTo(y.DirectoryAskQuality());
            }
        }
    }
}
