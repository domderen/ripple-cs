using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Formats;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Core.Types.Known.Sle;
using Ripple.Core.Core.Types.Known.Sle.Entries;
using Ripple.Core.Core.Types.Known.Tx;
using Ripple.Core.Core.Types.Known.Tx.Result;
using Ripple.Core.Core.Types.Known.Tx.Txns;
using Type = Ripple.Core.Core.Fields.Type;

namespace Ripple.Core.Core.Coretypes
{
    public class StObject : ISerializedType, IEnumerable<Field>
    {
        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.StObjectField TransactionMetaData = new StObjectField(Field.TransactionMetaData);
        public static TypedFields.StObjectField CreatedNode = new StObjectField(Field.CreatedNode);
        public static TypedFields.StObjectField DeletedNode = new StObjectField(Field.DeletedNode);
        public static TypedFields.StObjectField ModifiedNode = new StObjectField(Field.ModifiedNode);
        public static TypedFields.StObjectField PreviousFields = new StObjectField(Field.PreviousFields);
        public static TypedFields.StObjectField FinalFields = new StObjectField(Field.FinalFields);
        public static TypedFields.StObjectField NewFields = new StObjectField(Field.NewFields);
        public static TypedFields.StObjectField TemplateEntry = new StObjectField(Field.TemplateEntry);

        protected FieldsMap fields;

        public Format Format;

        public StObject()
        {
            fields = new FieldsMap();
        }

        public StObject(FieldsMap fieldsMap)
        {
            fields = fieldsMap;
        }


        public int Count
        {
            get { return fields.Count; }
        }

        public FieldsMap Fields
        {
            get { return fields; }
        }

        public AccountId this[TypedFields.AccountIdField f]
        {
            get { return (AccountId)this[f.GetField()]; }
        }

        public Amount this[TypedFields.AmountField f]
        {
            get { return (Amount)this[f.GetField()]; }
        }

        public StArray this[TypedFields.StArrayField f]
        {
            get { return (StArray)this[f.GetField()]; }
        }

        public Hash128 this[TypedFields.Hash128Field f]
        {
            get { return (Hash128)this[f.GetField()]; }
        }

        public Hash160 this[TypedFields.Hash160Field f]
        {
            get { return (Hash160)this[f.GetField()]; }
        }

        public Hash256 this[TypedFields.Hash256Field f]
        {
            get { return (Hash256) this[f.GetField()]; }
        }

        public StObject this[TypedFields.StObjectField f]
        {
            get
            {
                return (StObject)this[f.GetField()];
            }
        }

        public PathSet this[TypedFields.PathSetField f]
        {
            get
            {
                return (PathSet)this[f.GetField()];
            }
        }

        public UInt.UInt16 this[TypedFields.UInt16Field f]
        {
            get
            {
                return (UInt.UInt16)this[f.GetField()];
            }
        }

        public UInt.UInt32 this[TypedFields.UInt32Field f]
        {
            get
            {
                return (UInt.UInt32)this[f.GetField()];
            }
        }

        public UInt.UInt64 this[TypedFields.UInt64Field f]
        {
            get
            {
                return (UInt.UInt64)this[f.GetField()];
            }
        }

        public UInt8 this[TypedFields.UInt8Field f]
        {
            get
            {
                return (UInt8)this[f.GetField()];
            }
        }

        public Vector256 this[TypedFields.Vector256Field f]
        {
            get
            {
                return (Vector256)this[f.GetField()];
            }
        }

        public VariableLength this[TypedFields.VariableLengthField f]
        {
            get
            {
                return (VariableLength)this[f.GetField()];
            }
        }

        public ISerializedType this[Field field]
        {
            get
            {
                ISerializedType val;
                return this.fields.TryGetValue(field, out val) ? val : null;
            }
        }

        public Format GetFormat
        {
            get
            {
                if (Format == null)
                {
                    ComputeFormat();
                }

                return Format;
            }
        }

        public static StObject FromJson(string offerJson)
        {
            try
            {
                return FromJObject(new JObject(offerJson));
            }
            catch (JsonException e)
            {
                throw new ApplicationException("Json deserialization failed.", e);
            }
        }

        public static StObject NewInstance()
        {
            return new StObject();
        }

        public static StObject Formatted(StObject source)
        {
            if (AffectedNode.IsAffectedNode(source))
            {
                return new AffectedNode(source);
            }

            if (TransactionMeta.IsTransactionMeta(source))
            {
                var meta = new TransactionMeta { fields = source.fields };
                return meta;
            }

            var ledgerEntryType = LedgerEntryType(source);
            if (ledgerEntryType != null)
            {
                return LedgerFormatted(source, ledgerEntryType);
            }

            var transactionType = TransactionType(source);
            if (transactionType != null)
            {
                return TransactionFormatted(source, transactionType);
            }

            return source;
        }

        public static TransactionType TransactionType(StObject obj)
        {
            var uInt16 = obj[UInt.UInt16.TransactionType];

            if (uInt16 == null)
            {
                return null;
            }

            return Enums.TransactionType.FromNumber(uInt16);
        }

        public static LedgerEntryType LedgerEntryType(StObject obj)
        {
            UInt.UInt16 uInt16 = obj[UInt.UInt16.LedgerEntryType];
            if (uInt16 == null)
            {
                return null;
            }

            return Enums.LedgerEntryType.FromNumber(uInt16);
        }

        public string PrettyJson()
        {
            try
            {
                return InTranslate.ToJObject(this).ToString(Formatting.Indented);
            }
            catch (JsonException e)
            {
                throw new ApplicationException("Json serialization failed.", e);
            }
        }

        public object ToJson()
        {
            return InTranslate.ToJson(this);
        }

        public JObject ToJObject()
        {
            return InTranslate.ToJObject(this);
        }

        public byte[] ToBytes()
        {
            return InTranslate.ToBytes(this);
        }

        public string ToHex()
        {
            return InTranslate.ToHex(this);
        }

        public static StObject FromJObject(JObject json)
        {
            return OutTranslate.FromJObject(json);
        }

        public IEnumerator<Field> GetEnumerator()
        {
            return fields.Keys.GetEnumerator();
        }

        public void ToBytesSink(IBytesSink to)
        {
            ToBytesSink(to, new IsSerializedFieldFilter());
        }

        public void ToBytesSink(IBytesSink to, IFieldFilter p)
        {
            var serializer = new BinarySerializer(to);

            foreach (var field in this)
            {
                if (p.Evaluate(field))
                {
                    var value = fields[field];
                    serializer.Add(field, value);
                }
            }
        }


        // TODO, move these some where more specific
        // leave these here as static methods
        // delegate from more specific places
        public static TransactionEngineResult TransactionResult(StObject obj)
        {
            var uInt8 = obj[UInt8.TransactionResult];
            if (uInt8 == null)
            {
                return null;
            }

            return TransactionEngineResult.FromNumber(uInt8.IntValue());
        }

        public ISerializedType Remove(Field f)
        {
            if (fields.ContainsKey(f))
            {
                var field = this[f];
                fields.Remove(f);
                return field;
            }

            return null;
        }

        public bool Has(Field f)
        {
            return fields.ContainsKey(f);
        }

        public bool Has<T>(T hf) where T : IHasField
        {
            return Has(hf.GetField());
        }

        public void Add<T>(T f, object value) where T : IHasField
        {
            var s = value as string;
            if (s != null)
            {
                Add(f, s);
            }
            else
            {
                Add(f.GetField(), value);
            }
        }

        public void Add<T>(T hf, int i) where T : IHasField
        {
            Add(hf.GetField(), i);
        }

        public void Add(Field f, int i)
        {
            Add(f, OutTranslators.ForField(f).FromInteger(i));
        }

        public void Add<T>(T hf, string s) where T : IHasField
        {
            Add(hf.GetField(), s);    
        }

        public void Add<T>(T hf, byte[] bytes) where T : IHasField
        {
            var f = hf.GetField();
            Add(f, bytes);
        }

        public void Add(Field f, string s)
        {
            if (FieldSymbolics.IsSymbolicField(f))
            {
                Add(f, FieldSymbolics.AsInteger(f, s));
                return;
            }

            Add(f, OutTranslators.ForField(f).FromString(s));
        }

        public void Add(Field f, ISerializedType value)
        {
            fields.Add(f, value);
        }

        public void Add(Field f, object value)
        {
            var typeTranslator = OutTranslators.ForField(f);
            ISerializedType value1;

            try
            {
                value1 = typeTranslator.FromValue(value);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Couldn't add '{0}' into field '{1}\n{2}", value, f, e));
            }

            fields.Add(f, value1);
        }

        public void SetFormat(Format format)
        {
            Format = format;
        }

        private static StObject TransactionFormatted(StObject source, TransactionType transactionType)
        {
            StObject constructed = null;

            if (transactionType == Enums.TransactionType.Invalid)
            {               
            }
            else if (transactionType == Enums.TransactionType.Payment)
            {
                constructed = new Payment();
            }
            else if (transactionType == Enums.TransactionType.Claim)
            {
            }
            else if (transactionType == Enums.TransactionType.WalletAdd)
            {
            }
            else if (transactionType == Enums.TransactionType.AccountSet)
            {
                constructed = new AccountSet();
            }
            else if (transactionType == Enums.TransactionType.PasswordFund)
            {
            }
            else if (transactionType == Enums.TransactionType.SetRegularKey)
            {
            }
            else if (transactionType == Enums.TransactionType.NickNameSet)
            {
            }
            else if (transactionType == Enums.TransactionType.OfferCreate)
            {
                constructed = new OfferCreate();
            }
            else if (transactionType == Enums.TransactionType.OfferCancel)
            {
                constructed = new OfferCancel();
            }
            else if (transactionType == Enums.TransactionType.Contract)
            {
            }
            else if (transactionType == Enums.TransactionType.RemoveContract)
            {
            }
            else if (transactionType == Enums.TransactionType.TrustSet)
            {
                constructed = new TrustSet();
            }
            else if (transactionType == Enums.TransactionType.EnableFeature)
            {
            }
            else if (transactionType == Enums.TransactionType.SetFee)
            {
            }

            if (constructed == null)
            {
                constructed = new Transaction(transactionType);
            }

            constructed.fields = source.fields;

            return constructed;
        }

        private static StObject LedgerFormatted(StObject source, LedgerEntryType ledgerEntryType)
        {
            StObject constructed = null;

            if (ledgerEntryType == Enums.LedgerEntryType.Offer)
            {
                constructed = new Offer();
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.RippleState)
            {
                constructed = new RippleState();
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.AccountRoot)
            {
                constructed = new AccountRoot();
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.Invalid)
            {
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.DirectoryNode)
            {
                constructed = new DirectoryNode();
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.GeneratorMap)
            {
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.Nickname)
            {
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.Contract)
            {
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.LedgerHashes)
            {
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.EnabledFeatures)
            {
            }
            else if (ledgerEntryType == Enums.LedgerEntryType.FeeSettings)
            {
            }

            if (constructed == null)
            {
                constructed = new LedgerEntry(ledgerEntryType);
            }

            constructed.fields = source.fields;

            return constructed;
        }

        private void ComputeFormat()
        {
            var tt = this[UInt.UInt16.TransactionType];

            if (tt != null)
            {
                SetFormat(TxFormat.FromNumber(tt));
            }

            var let = this[UInt.UInt16.LedgerEntryType];

            if (let != null)
            {
                SetFormat(TxFormat.FromNumber(let));
            }
        }

        private void Add(Field f, byte[] bytes)
        {
            // TODO: all!!! (?)
            Add(f, OutTranslators.ForField(f).FromBytes(bytes));
        }

        public interface IFieldFilter
        {
            bool Evaluate(Field a);
        }

        public class FieldsMap : Dictionary<Field, ISerializedType> { }

        public class OutTranslator : OutTypeTranslator<StObject>
        {
            public override StObject FromParser(BinaryParser parser, int? hint)
            {
                var so = new StObject();
                IOutTypeTranslator<ISerializedType> tr;
                ISerializedType st;
                Field field;
                int? sizeHint;

                while (!parser.End)
                {
                    field = parser.ReadField();
                    if (field == Field.ObjectEndMarker)
                    {
                        break;
                    }

                    tr = OutTranslators.ForField(field);
                    sizeHint = field.IsVlEncoded() ? parser.ReadVlLength() : (int?)null;
                    st = tr.FromParser(parser, sizeHint);
                    if (st == null)
                    {
                        throw new InvalidOperationException(string.Format("Parsed {0} as null", field));
                    }
                    so.Add(field, st);
                }

                return Formatted(so);
            }

            public override StObject FromJObject(JObject jsonObject)
            {
                var so = new StObject();

                var keys = jsonObject.GetEnumerator();
                while (keys.MoveNext())
                {
                    var key = keys.Current.Key;
                    try
                    {
                        object value = jsonObject[key];
                        Field fieldKey = Field.FromString(key);
                        if (fieldKey == null)
                        {
                            // TODO test for UpperCase key name
                            // warn about possibly unknown field
                            continue;
                        }

                        if (FieldSymbolics.IsSymbolicField(fieldKey) && ((JToken)value).Type == JTokenType.String)
                        {
                            value = FieldSymbolics.AsInteger(fieldKey, value.ToString());
                        }

                        if (value is JToken && ((JToken)value).Type == JTokenType.Integer)
                        {
                            so.Add(fieldKey, ((JToken)value).ToObject<int>());
                        }
                        else if (value is JToken && ((JToken)value).Type == JTokenType.String)
                        {
                            so.Add(fieldKey, ((JToken)value).ToString());
                        }
                        else
                        {
                            so.Add(fieldKey, value);
                        }
                    }
                    catch (JsonException e)
                    {
                        throw new ApplicationException("Json deserialization failed.", e);
                    }
                }
                return Formatted(so);
            }
        }

        public class InTranslator : InTypeTranslator<StObject>
        {
            public override object ToJson(StObject obj)
            {
                return ToJObject(obj);
            }

            public override JObject ToJObject(StObject obj)
            {
                var json = new JObject();

                foreach (Field f in obj)
                {
                    try
                    {
                        ISerializedType obj1 = obj[f];
                        Object someObject = obj1.ToJson();

                        if (someObject is JToken)
                        {
                            if (FieldSymbolics.IsSymbolicField(f) && ((JToken)someObject).Type == JTokenType.Integer)
                            {
                                someObject = FieldSymbolics.AsString(f, ((JToken)someObject).ToObject<int>());
                            }

                            json.Add(Field.GetName(f), (JToken)someObject);
                        }
                        else if (FieldSymbolics.IsSymbolicField(f) && someObject is int)
                        {
                            someObject = FieldSymbolics.AsString(f, (int)someObject);
                            json.Add(Field.GetName(f), someObject.ToString());
                        }
                        else if (someObject is int)
                        {
                            json.Add(Field.GetName(f), (int)someObject);
                        }
                        else
                        {
                            json.Add(Field.GetName(f), someObject.ToString());
                        }
                        
                    }
                    catch (JsonException e)
                    {
                        throw new ApplicationException("Json serialization failed.", e);
                    }
                }

                return json;
            }
        }

        public static class OutTranslators
        {
            public static IOutTypeTranslator<ISerializedType> ForField(Field field)
            {
                if (field.Tag == null)
                {
                    var fieldType = field.Type;
                    if (fieldType == Type.StObject) field.Tag = OutTranslate;
                    else if (fieldType == Type.Amount) field.Tag = Amount.OutTranslate;
                    else if (fieldType == Type.UInt16) field.Tag = UInt.UInt16.OutTranslate;
                    else if (fieldType == Type.UInt32) field.Tag = UInt.UInt32.OutTranslate;
                    else if (fieldType == Type.UInt64) field.Tag = UInt.UInt64.OutTranslate;
                    else if (fieldType == Type.Hash128) field.Tag = Hash128.OutTranslate;
                    else if (fieldType == Type.Hash256) field.Tag = Hash256.OutTranslate;
                    else if (fieldType == Type.VariableLength) field.Tag = VariableLength.OutTranslate;
                    else if (fieldType == Type.AccountId) field.Tag = AccountId.OutTranslate;
                    else if (fieldType == Type.StArray) field.Tag = StArray.OutTranslate;
                    else if (fieldType == Type.UInt8) field.Tag = UInt8.OutTranslate;
                    else if (fieldType == Type.Hash160) field.Tag = Hash160.OutTranslate;
                    else if (fieldType == Type.PathSet) field.Tag = PathSet.OutTranslate;
                    else if (fieldType == Type.Vector256) field.Tag = Vector256.OutTranslate;
                    else throw new ApplicationException("Unknown type.");
                }

                return GetCastedTag(field);
            }

            private static IOutTypeTranslator<ISerializedType> GetCastedTag(Field field)
            {
                unchecked
                {
                    return (IOutTypeTranslator<ISerializedType>)field.Tag; 
                }
            }
        }

        private class StObjectField : TypedFields.StObjectField
        {
            private readonly Field _f;

            public StObjectField(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }

        private class IsSerializedFieldFilter : IFieldFilter
        {
            public bool Evaluate(Field a)
            {
                return a.IsSerialized();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
