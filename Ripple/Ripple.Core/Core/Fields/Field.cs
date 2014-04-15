using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ripple.Core.Core.Fields
{
    public class Field
    {
        private static readonly Dictionary<string, Field> Values = new Dictionary<string, Field>();
        private static readonly Dictionary<int, Field> ByCode = new Dictionary<int, Field>();
        private readonly int _id;
        private readonly Type _type;
        private readonly int _code;
        private readonly byte[] _bytes;
        public object Tag = null;

        static Field()
        {
            Values.Add("Generic", Generic);
            Values.Add("Invalid", Invalid);
            Values.Add("LedgerEntryType", LedgerEntryType);
            Values.Add("TransactionType", TransactionType);
            Values.Add("Flags", Flags);
            Values.Add("SourceTag", SourceTag);
            Values.Add("Sequence", Sequence);
            Values.Add("PreviousTxnLgrSeq", PreviousTxnLgrSeq);
            Values.Add("LedgerSequence", LedgerSequence);
            Values.Add("CloseTime", CloseTime);
            Values.Add("ParentCloseTime", ParentCloseTime);
            Values.Add("SigningTime", SigningTime);
            Values.Add("Expiration", Expiration);
            Values.Add("TransferRate", TransferRate);
            Values.Add("WalletSize", WalletSize);
            Values.Add("OwnerCount", OwnerCount);
            Values.Add("DestinationTag", DestinationTag);
            Values.Add("HighQualityIn", HighQualityIn);
            Values.Add("HighQualityOut", HighQualityOut);
            Values.Add("LowQualityIn", LowQualityIn);
            Values.Add("LowQualityOut", LowQualityOut);
            Values.Add("QualityIn", QualityIn);
            Values.Add("QualityOut", QualityOut);
            Values.Add("StampEscrow", StampEscrow);
            Values.Add("BondAmount", BondAmount);
            Values.Add("LoadFee", LoadFee);
            Values.Add("OfferSequence", OfferSequence);
            Values.Add("FirstLedgerSequence", FirstLedgerSequence);
            Values.Add("LastLedgerSequence", LastLedgerSequence);
            Values.Add("TransactionIndex", TransactionIndex);
            Values.Add("OperationLimit", OperationLimit);
            Values.Add("ReferenceFeeUnits", ReferenceFeeUnits);
            Values.Add("ReserveBase", ReserveBase);
            Values.Add("ReserveIncrement", ReserveIncrement);
            Values.Add("SetFlag", SetFlag);
            Values.Add("ClearFlag", ClearFlag);
            Values.Add("IndexNext", IndexNext);
            Values.Add("IndexPrevious", IndexPrevious);
            Values.Add("BookNode", BookNode);
            Values.Add("OwnerNode", OwnerNode);
            Values.Add("BaseFee", BaseFee);
            Values.Add("ExchangeRate", ExchangeRate);
            Values.Add("LowNode", LowNode);
            Values.Add("HighNode", HighNode);
            Values.Add("EmailHash", EmailHash);
            Values.Add("LedgerHash", LedgerHash);
            Values.Add("ParentHash", ParentHash);
            Values.Add("TransactionHash", TransactionHash);
            Values.Add("AccountHash", AccountHash);
            Values.Add("PreviousTxnID", PreviousTxnID);
            Values.Add("LedgerIndex", LedgerIndex);
            Values.Add("WalletLocator", WalletLocator);
            Values.Add("RootIndex", RootIndex);
            Values.Add("AccountTxnID", AccountTxnID);
            Values.Add("BookDirectory", BookDirectory);
            Values.Add("InvoiceID", InvoiceID);
            Values.Add("Nickname", Nickname);
            Values.Add("Feature", Feature);
            Values.Add("hash", hash);
            Values.Add("index", index);
            Values.Add("Amount", Amount);
            Values.Add("Balance", Balance);
            Values.Add("LimitAmount", LimitAmount);
            Values.Add("TakerPays", TakerPays);
            Values.Add("TakerGets", TakerGets);
            Values.Add("LowLimit", LowLimit);
            Values.Add("HighLimit", HighLimit);
            Values.Add("Fee", Fee);
            Values.Add("SendMax", SendMax);
            Values.Add("MinimumOffer", MinimumOffer);
            Values.Add("RippleEscrow", RippleEscrow);
            Values.Add("DeliveredAmount", DeliveredAmount);
            Values.Add("PublicKey", PublicKey);
            Values.Add("MessageKey", MessageKey);
            Values.Add("SigningPubKey", SigningPubKey);
            Values.Add("TxnSignature", TxnSignature);
            Values.Add("Generator", Generator);
            Values.Add("Signature", Signature);
            Values.Add("Domain", Domain);
            Values.Add("FundCode", FundCode);
            Values.Add("RemoveCode", RemoveCode);
            Values.Add("ExpireCode", ExpireCode);
            Values.Add("CreateCode", CreateCode);
            Values.Add("Account", Account);
            Values.Add("Owner", Owner);
            Values.Add("Destination", Destination);
            Values.Add("Issuer", Issuer);
            Values.Add("Target", Target);
            Values.Add("RegularKey", RegularKey);
            Values.Add("ObjectEndMarker", ObjectEndMarker);
            Values.Add("TransactionMetaData", TransactionMetaData);
            Values.Add("CreatedNode", CreatedNode);
            Values.Add("DeletedNode", DeletedNode);
            Values.Add("ModifiedNode", ModifiedNode);
            Values.Add("PreviousFields", PreviousFields);
            Values.Add("FinalFields", FinalFields);
            Values.Add("NewFields", NewFields);
            Values.Add("TemplateEntry", TemplateEntry);
            Values.Add("ArrayEndMarker", ArrayEndMarker);
            Values.Add("SigningAccounts", SigningAccounts);
            Values.Add("TxnSignatures", TxnSignatures);
            Values.Add("Signatures", Signatures);
            Values.Add("Template", Template);
            Values.Add("Necessary", Necessary);
            Values.Add("Sufficient", Sufficient);
            Values.Add("AffectedNodes", AffectedNodes);
            Values.Add("CloseResolution", CloseResolution);
            Values.Add("TemplateEntryType", TemplateEntryType);
            Values.Add("TransactionResult", TransactionResult);
            Values.Add("TakerPaysCurrency", TakerPaysCurrency);
            Values.Add("TakerPaysIssuer", TakerPaysIssuer);
            Values.Add("TakerGetsCurrency", TakerGetsCurrency);
            Values.Add("TakerGetsIssuer", TakerGetsIssuer);
            Values.Add("Paths", Paths);
            Values.Add("Indexes", Indexes);
            Values.Add("Hashes", Hashes);
            Values.Add("Features", Features);
            Values.Add("Transaction", Transaction);
            Values.Add("LedgerEntry", LedgerEntry);
            Values.Add("Validation", Validation);

            foreach (var value in Values)
            {
                if (ByCode.ContainsKey(value.Value._code))
                {
                    ByCode[value.Value._code] = value.Value;
                }
                else
                {
                    ByCode.Add(value.Value._code, value.Value);
                }
            }

            var values = new Field[Values.Count];
            Array.Copy(Values.Values.ToArray(), values, Values.Count);
            var sortedFields = new List<Field>(values);
            sortedFields.Sort(Comparator);

            for (int i = 0; i < values.Length; i++)
            {
                var av = values[i];
                var lv = sortedFields.ElementAt(i);
                if (av._code != lv._code)
                {
                    throw new ApplicationException("Field class declaration isn't presorted.");
                }
            }
        }

        private Field(int fid, Type tid)
        {
            _id = fid;
            _type = tid;
            _code = (_type.Id << 16) | fid;
            _bytes = IsSerialized() ? AsBytes(this) : null;
        }

        public static Comparison<Field> Comparator = (field, field1) => field._code - field1._code;

        // These are all presorted (verified in a static block below)
        // They can then be used in a TreeMap, using the Enum (private) ordinal
        // comparator
        public static readonly Field Generic = new Field(0, Type.Unknown);
        public static readonly Field Invalid = new Field(-1, Type.Unknown);
        public static readonly Field LedgerEntryType = new Field(1, Type.UInt16);
        public static readonly Field TransactionType = new Field(2, Type.UInt16);
        public static readonly Field Flags = new Field(2, Type.UInt32);
        public static readonly Field SourceTag = new Field(3, Type.UInt32);
        public static readonly Field Sequence = new Field(4, Type.UInt32);
        public static readonly Field PreviousTxnLgrSeq = new Field(5, Type.UInt32);
        public static readonly Field LedgerSequence = new Field(6, Type.UInt32);
        public static readonly Field CloseTime = new Field(7, Type.UInt32);
        public static readonly Field ParentCloseTime = new Field(8, Type.UInt32);
        public static readonly Field SigningTime = new Field(9, Type.UInt32);
        public static readonly Field Expiration = new Field(10, Type.UInt32);
        public static readonly Field TransferRate = new Field(11, Type.UInt32);
        public static readonly Field WalletSize = new Field(12, Type.UInt32);
        public static readonly Field OwnerCount = new Field(13, Type.UInt32);
        public static readonly Field DestinationTag = new Field(14, Type.UInt32);
        public static readonly Field HighQualityIn = new Field(16, Type.UInt32);
        public static readonly Field HighQualityOut = new Field(17, Type.UInt32);
        public static readonly Field LowQualityIn = new Field(18, Type.UInt32);
        public static readonly Field LowQualityOut = new Field(19, Type.UInt32);
        public static readonly Field QualityIn = new Field(20, Type.UInt32);
        public static readonly Field QualityOut = new Field(21, Type.UInt32);
        public static readonly Field StampEscrow = new Field(22, Type.UInt32);
        public static readonly Field BondAmount = new Field(23, Type.UInt32);
        public static readonly Field LoadFee = new Field(24, Type.UInt32);
        public static readonly Field OfferSequence = new Field(25, Type.UInt32);

        /// <summary>
        /// Deprecated: do not use.
        /// </summary>
        public static readonly Field FirstLedgerSequence = new Field(26, Type.UInt32);

        /// <summary>
        /// Added new semantics in 9486fc416ca7c59b8930b734266eed4d5b714c50.
        /// </summary>
        public static readonly Field LastLedgerSequence = new Field(27, Type.UInt32);
        public static readonly Field TransactionIndex = new Field(28, Type.UInt32);
        public static readonly Field OperationLimit = new Field(29, Type.UInt32);
        public static readonly Field ReferenceFeeUnits = new Field(30, Type.UInt32);
        public static readonly Field ReserveBase = new Field(31, Type.UInt32);
        public static readonly Field ReserveIncrement = new Field(32, Type.UInt32);
        public static readonly Field SetFlag = new Field(33, Type.UInt32);
        public static readonly Field ClearFlag = new Field(34, Type.UInt32);
        public static readonly Field IndexNext = new Field(1, Type.UInt64);
        public static readonly Field IndexPrevious = new Field(2, Type.UInt64);
        public static readonly Field BookNode = new Field(3, Type.UInt64);
        public static readonly Field OwnerNode = new Field(4, Type.UInt64);
        public static readonly Field BaseFee = new Field(5, Type.UInt64);
        public static readonly Field ExchangeRate = new Field(6, Type.UInt64);
        public static readonly Field LowNode = new Field(7, Type.UInt64);
        public static readonly Field HighNode = new Field(8, Type.UInt64);
        public static readonly Field EmailHash = new Field(1, Type.Hash128);
        public static readonly Field LedgerHash = new Field(1, Type.Hash256);
        public static readonly Field ParentHash = new Field(2, Type.Hash256);
        public static readonly Field TransactionHash = new Field(3, Type.Hash256);
        public static readonly Field AccountHash = new Field(4, Type.Hash256);
        public static readonly Field PreviousTxnID = new Field(5, Type.Hash256);
        public static readonly Field LedgerIndex = new Field(6, Type.Hash256);
        public static readonly Field WalletLocator = new Field(7, Type.Hash256);
        public static readonly Field RootIndex = new Field(8, Type.Hash256);

        /// <summary>
        /// Added in rippled commit: 9486fc416ca7c59b8930b734266eed4d5b714c50.
        /// </summary>
        public static readonly Field AccountTxnID = new Field(9, Type.Hash256);
        public static readonly Field BookDirectory = new Field(16, Type.Hash256);
        public static readonly Field InvoiceID = new Field(17, Type.Hash256);
        public static readonly Field Nickname = new Field(18, Type.Hash256);
        public static readonly Field Feature = new Field(19, Type.Hash256);
        public static readonly Field hash = new Field(257, Type.Hash256);
        public static readonly Field index = new Field(258, Type.Hash256);

        public static readonly Field Amount = new Field(1, Type.Amount);
        public static readonly Field Balance = new Field(2, Type.Amount);
        public static readonly Field LimitAmount = new Field(3, Type.Amount);
        public static readonly Field TakerPays = new Field(4, Type.Amount);
        public static readonly Field TakerGets = new Field(5, Type.Amount);
        public static readonly Field LowLimit = new Field(6, Type.Amount);
        public static readonly Field HighLimit = new Field(7, Type.Amount);
        public static readonly Field Fee = new Field(8, Type.Amount);
        public static readonly Field SendMax = new Field(9, Type.Amount);
        public static readonly Field MinimumOffer = new Field(16, Type.Amount);
        public static readonly Field RippleEscrow = new Field(17, Type.Amount);

        /// <summary>
        /// Added in rippled commit: e7f0b8eca69dd47419eee7b82c8716b3aa5a9e39.
        /// </summary>
        public static readonly Field DeliveredAmount = new Field(18, Type.Amount);

        public static readonly Field PublicKey = new Field(1, Type.VariableLength);
        public static readonly Field MessageKey = new Field(3, Type.VariableLength);
        public static readonly Field SigningPubKey = new Field(3, Type.VariableLength);
        public static readonly Field TxnSignature = new Field(4, Type.VariableLength);
        public static readonly Field Generator = new Field(5, Type.VariableLength);
        public static readonly Field Signature = new Field(6, Type.VariableLength);
        public static readonly Field Domain = new Field(7, Type.VariableLength);
        public static readonly Field FundCode = new Field(8, Type.VariableLength);
        public static readonly Field RemoveCode = new Field(9, Type.VariableLength);
        public static readonly Field ExpireCode = new Field(10, Type.VariableLength);
        public static readonly Field CreateCode = new Field(11, Type.VariableLength);
        public static readonly Field Account = new Field(1, Type.AccountId);
        public static readonly Field Owner = new Field(2, Type.AccountId);
        public static readonly Field Destination = new Field(3, Type.AccountId);
        public static readonly Field Issuer = new Field(4, Type.AccountId);
        public static readonly Field Target = new Field(7, Type.AccountId);
        public static readonly Field RegularKey = new Field(8, Type.AccountId);
        public static readonly Field ObjectEndMarker = new Field(1, Type.StObject);
        public static readonly Field TransactionMetaData = new Field(2, Type.StObject);
        public static readonly Field CreatedNode = new Field(3, Type.StObject);
        public static readonly Field DeletedNode = new Field(4, Type.StObject);
        public static readonly Field ModifiedNode = new Field(5, Type.StObject);
        public static readonly Field PreviousFields = new Field(6, Type.StObject);
        public static readonly Field FinalFields = new Field(7, Type.StObject);
        public static readonly Field NewFields = new Field(8, Type.StObject);
        public static readonly Field TemplateEntry = new Field(9, Type.StObject);
        public static readonly Field ArrayEndMarker = new Field(1, Type.StArray);
        public static readonly Field SigningAccounts = new Field(2, Type.StArray);
        public static readonly Field TxnSignatures = new Field(3, Type.StArray);
        public static readonly Field Signatures = new Field(4, Type.StArray);
        public static readonly Field Template = new Field(5, Type.StArray);
        public static readonly Field Necessary = new Field(6, Type.StArray);
        public static readonly Field Sufficient = new Field(7, Type.StArray);
        public static readonly Field AffectedNodes = new Field(8, Type.StArray);
        public static readonly Field CloseResolution = new Field(1, Type.UInt8);
        public static readonly Field TemplateEntryType = new Field(2, Type.UInt8);
        public static readonly Field TransactionResult = new Field(3, Type.UInt8);
        public static readonly Field TakerPaysCurrency = new Field(1, Type.Hash160);
        public static readonly Field TakerPaysIssuer = new Field(2, Type.Hash160);
        public static readonly Field TakerGetsCurrency = new Field(3, Type.Hash160);
        public static readonly Field TakerGetsIssuer = new Field(4, Type.Hash160);
        public static readonly Field Paths = new Field(1, Type.PathSet);
        public static readonly Field Indexes = new Field(1, Type.Vector256);
        public static readonly Field Hashes = new Field(2, Type.Vector256);
        public static readonly Field Features = new Field(3, Type.Vector256);
        public static readonly Field Transaction = new Field(1, Type.Transaction);
        public static readonly Field LedgerEntry = new Field(1, Type.LedgerEntry);
        public static readonly Field Validation = new Field(1, Type.Validation);

        public int Id
        {
            get { return _id; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public byte[] Bytes
        {
            get { return _bytes; }
        }

        public static string GetName(Field f)
        {
            return Values.SingleOrDefault(q => q.Value == f).Key;
        }

        public static IEnumerator<Field> Sorted(Collection<Field> fields)
        {
            var fieldList = new List<Field>(fields);
            fieldList.Sort(Comparator);
            return fieldList.GetEnumerator();
        }

        public static Field FromString(string key)
        {
            return Values.FirstOrDefault(q => q.Key == key).Value;
        }

        public static Field FromCode(int integer)
        {
            return ByCode[integer];
        }

        public static byte[] AsBytes(Field field)
        {
            int name = field.Id;
            int type = field.Type.Id;

            var header = new List<byte>(3);

            if (type < 16)
            {
                if (name < 16) // Common type, common name.
                {
                    header.Add((byte)((type << 4) | name));
                }
                else
                {
                    // Common type, uncommon name.
                    header.Add((byte)(type << 4));
                    header.Add((byte)(name));
                }
            }
            else if (name < 16)
            {
                // Uncommon type, common name.
                header.Add((byte)(name));
                header.Add((byte)(type));
            }
            else
            {
                // Uncommon type, uncommon name.
                header.Add(0);
                header.Add((byte)(type));
                header.Add((byte)(name));
            }

            var headerBytes = new byte[header.Count()];
            for (int i = 0; i < header.Count; i++)
            {
                headerBytes[i] = header.ElementAt(i);
            }

            return headerBytes;
        }

        public bool IsSerialized()
        {
            // This should screen out "hash" and "index".
            return ((_type.Id > 0) && (_type.Id < 256) && (_id > 0) && (_id < 256));
        }

        public bool IsVlEncoded()
        {
            return _type == Type.VariableLength || _type == Type.AccountId || _type == Type.Vector256;
        }

        public bool IsSigningField()
        {
            return IsSerialized() && this != TxnSignature;
        }
    }
}
