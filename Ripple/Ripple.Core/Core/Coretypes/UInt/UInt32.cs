using Deveel.Math;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.UInt
{
    public class UInt32 : UInt<UInt32>
    {
        public UInt32(byte[] bytes)
            : base(bytes)
        {
        }

        public UInt32(BigInteger value)
            : base(value)
        {
        }

        public UInt32(long s)
            : base(s)
        {
        }

        public UInt32(string s)
            : base(s)
        {
        }

        public UInt32(string s, int radix)
            : base(s, radix)
        {
        }

        public static OutTypeTranslator<UInt32> OutTranslate = new OutUInt32Translator();
        public static InTypeTranslator<UInt32> InTranslate = new InUInt32Translator();
        public static TypedFields.UInt32Field Flags = new UInt32Field(Field.Flags);
        public static TypedFields.UInt32Field SourceTag = new UInt32Field(Field.SourceTag);
        public static TypedFields.UInt32Field Sequence = new UInt32Field(Field.Sequence);
        public static TypedFields.UInt32Field PreviousTxnLgrSeq = new UInt32Field(Field.PreviousTxnLgrSeq);
        public static TypedFields.UInt32Field LedgerSequence = new UInt32Field(Field.LedgerSequence);
        public static TypedFields.UInt32Field CloseTime = new UInt32Field(Field.CloseTime);
        public static TypedFields.UInt32Field ParentCloseTime = new UInt32Field(Field.ParentCloseTime);
        public static TypedFields.UInt32Field SigningTime = new UInt32Field(Field.SigningTime);
        public static TypedFields.UInt32Field Expiration = new UInt32Field(Field.Expiration);
        public static TypedFields.UInt32Field TransferRate = new UInt32Field(Field.TransferRate);
        public static TypedFields.UInt32Field WalletSize = new UInt32Field(Field.WalletSize);
        public static TypedFields.UInt32Field OwnerCount = new UInt32Field(Field.OwnerCount);
        public static TypedFields.UInt32Field DestinationTag = new UInt32Field(Field.DestinationTag);
        public static TypedFields.UInt32Field HighQualityIn = new UInt32Field(Field.HighQualityIn);
        public static TypedFields.UInt32Field HighQualityOut = new UInt32Field(Field.HighQualityOut);
        public static TypedFields.UInt32Field LowQualityIn = new UInt32Field(Field.LowQualityIn);
        public static TypedFields.UInt32Field LowQualityOut = new UInt32Field(Field.LowQualityOut);
        public static TypedFields.UInt32Field QualityIn = new UInt32Field(Field.QualityIn);
        public static TypedFields.UInt32Field QualityOut = new UInt32Field(Field.QualityOut);
        public static TypedFields.UInt32Field StampEscrow = new UInt32Field(Field.StampEscrow);
        public static TypedFields.UInt32Field BondAmount = new UInt32Field(Field.BondAmount);
        public static TypedFields.UInt32Field LoadFee = new UInt32Field(Field.LoadFee);
        public static TypedFields.UInt32Field OfferSequence = new UInt32Field(Field.OfferSequence);
        public static TypedFields.UInt32Field FirstLedgerSequence = new UInt32Field(Field.FirstLedgerSequence);
        public static TypedFields.UInt32Field LastLedgerSequence = new UInt32Field(Field.LastLedgerSequence);
        public static TypedFields.UInt32Field TransactionIndex = new UInt32Field(Field.TransactionIndex);
        public static TypedFields.UInt32Field OperationLimit = new UInt32Field(Field.OperationLimit);
        public static TypedFields.UInt32Field ReferenceFeeUnits = new UInt32Field(Field.ReferenceFeeUnits);
        public static TypedFields.UInt32Field ReserveBase = new UInt32Field(Field.ReserveBase);
        public static TypedFields.UInt32Field ReserveIncrement = new UInt32Field(Field.ReserveIncrement);
        public static TypedFields.UInt32Field SetFlag = new UInt32Field(Field.SetFlag);
        public static TypedFields.UInt32Field ClearFlag = new UInt32Field(Field.ClearFlag);

        public override int GetByteWidth()
        {
            return 4;
        }

        public override UIntBase InstanceFrom(BigInteger n)
        {
            return new UInt32(n);
        }

        public override object Value()
        {
            return LongValue();
        }

        public override object ToJson()
        {
            return InTranslate.ToJson(this);
        }

        public override byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public override string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public override void ToBytesSink(IBytesSink to)
        {
            InTranslate.ToBytesSink(this, to);
        }

        private class OutUInt32Translator : OutUIntTranslator<UInt32>
        {
            public override UInt32 NewInstance(BigInteger i)
            {
                return new UInt32(i);
            }

            public override int ByteWidth()
            {
                return 4;
            }
        }

        private class InUInt32Translator : InUIntTranslator<UInt32>
        {
        }

        private class UInt32Field : TypedFields.UInt32Field
        {
            private readonly Field _f;

            public UInt32Field(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }
    }
}
