using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Coretypes.UInt;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Fields;
using Ripple.Core.Encodings.Common;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx.Result
{
    public class TransactionResult
    {
        public TransactionEngineResult EngineResult;
        public Hash256 LedgerHash;
        public Hash256 Hash;

        public UInt32 LedgerIndex;
        public bool Validated;

        public Transaction Txn;
        public TransactionMeta Meta;
        public JObject Message;

        public TransactionResult(JObject json, Source resultMessageSource)
        {
            Message = json;

            try
            {
                if (resultMessageSource == Source.transaction_subscription_notification)
                {
                    EngineResult = TransactionEngineResult.FromString(json.GetValue("engine_result").ToObject<string>());
                    Validated = json.GetValue("validated").ToObject<bool>();
                    LedgerHash = Hash256.Translate.FromString(json.GetValue("ledger_hash").ToObject<string>());
                    LedgerIndex = new UInt32(json.GetValue("ledger_index").ToObject<long>());

                    JToken transaction;
                    if (json.TryGetValue("transaction", out transaction))
                    {
                        Txn = (Transaction) StObject.FromJObject(transaction.ToObject<JObject>());
                        Hash = Txn[Hash256.hash];
                    }

                    JToken meta;
                    if (json.TryGetValue("meta", out meta))
                    {
                        Meta = (TransactionMeta)StObject.FromJObject(meta.ToObject<JObject>());
                    }
                }
                else if (resultMessageSource == Source.request_tx_result)
                {
                    JToken validated;
                    Validated = json.TryGetValue("validated", out validated) && validated.ToObject<bool>();

                    JToken meta;
                    if (Validated && !json.TryGetValue("meta", out meta))
                    {
                        throw new InvalidOperationException("It's validated, why doesn't it have meta??");
                    }

                    if (Validated)
                    {
                        Meta = (TransactionMeta)StObject.FromJObject(json.GetValue("meta").ToObject<JObject>());
                        EngineResult = TransactionEngineResult.FromNumber(Meta[UInt8.TransactionResult]);
                        Txn = (Transaction)StObject.FromJObject(json);
                        Hash = Txn[Hash256.hash];
                        LedgerHash = null; // XXXXXX
                    }
                }
                else if (resultMessageSource == Source.request_account_tx)
                {
                    JToken validated;
                    Validated = json.TryGetValue("validated", out validated) && validated.ToObject<bool>();

                    JToken meta;
                    if (Validated && !json.TryGetValue("meta", out meta))
                    {
                        throw new InvalidOperationException("It's validated, why doesn't it have meta??");
                    }

                    if (Validated)
                    {
                        var tx = json.GetValue("tx").ToObject<JObject>();
                        Meta = (TransactionMeta)StObject.FromJObject(json.GetValue("meta").ToObject<JObject>());
                        EngineResult = TransactionEngineResult.FromNumber(Meta[UInt8.TransactionResult]);
                        Txn = (Transaction)StObject.FromJObject(tx);
                        Hash = Txn[Hash256.hash];
                        LedgerIndex = new UInt32(tx.GetValue("ledger_index").ToObject<long>());
                        LedgerHash = null;
                    }
                }
                else if (resultMessageSource == Source.request_account_tx_binary)
                {
                    JToken validated;
                    Validated = json.TryGetValue("validated", out validated) && validated.ToObject<bool>();

                    JToken meta;
                    if (Validated && !json.TryGetValue("meta", out meta))
                    {
                        throw new InvalidOperationException("It's validated, why doesn't it have meta??");
                    }

                    if (Validated)
                    {
                        /*
                        {
                            "ledger_index": 3378767,
                            "meta": "201 ...",
                            "tx_blob": "120 ...",
                            "validated": true
                        },
                        */

                        var tx = json.GetValue("tx_blob").ToObject<string>();
                        byte[] decodedTx = B16.Decode(tx);
                        Meta = (TransactionMeta)StObject.Translate.FromHex(json.GetValue("meta").ToObject<string>());
                        Txn = (Transaction)StObject.Translate.FromBytes(decodedTx);
                        Hash = Hash256.TransactionId(decodedTx);
                        Txn.Add(Field.hash, Hash);

                        EngineResult = Meta.TransactionResult();
                        LedgerIndex = new UInt32(json.GetValue("ledger_index").ToObject<long>());
                        LedgerHash = null;
                    }
                }
            }
            catch (JsonException e)
            {
                throw new ApplicationException("Json deserialization failed.", e);
            }
        }

        public TransactionType TransactionType
        {
            get { return Txn.TransactionType(); }
        }

        public bool IsPayment
        {
            get { return TransactionType == TransactionType.Payment; }
        }

        public AccountId InitiatingAccount()
        {
            return Txn[AccountId.Account];
        }

        public AccountId CreatedAccount()
        {
            AccountId destination = null;
            Hash256 destinationIndex = null;

            if (TransactionType == TransactionType.Payment && Meta.Has(Field.AffectedNodes))
            {
                StArray affected = Meta[StArray.AffectedNodes];

                foreach (var node in affected)
                {
                    if (node.Has(StObject.CreatedNode))
                    {
                        StObject created = node[StObject.CreatedNode];

                        if (StObject.LedgerEntryType(created) == LedgerEntryType.AccountRoot)
                        {
                            if (destination == null)
                            {
                                destination = Txn[AccountId.Destination];
                                destinationIndex = Hash256.AccountIdLedgerIndex(destination);
                            }

                            if (destinationIndex.Equals(created[Hash256.LedgerHash]))
                            {
                                return destination;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public IDictionary<AccountId, StObject> ModifiedRoots()
        {
            Dictionary<AccountId, StObject> accounts = null;

            if (Meta.Has(Field.AffectedNodes))
            {
                accounts = new Dictionary<AccountId, StObject>();

                StArray affected = Meta[StArray.AffectedNodes];
                foreach (var node in affected)
                {
                    if (node.Has(Field.ModifiedNode))
                    {
                        var localNode = node[StObject.ModifiedNode];
                        if (StObject.LedgerEntryType(localNode) == LedgerEntryType.AccountRoot)
                        {
                            StObject finalFields = localNode[StObject.FinalFields];
                            AccountId key;

                            if (finalFields != null)
                            {
                                key = finalFields[AccountId.Account];
                                accounts.Add(key, localNode);
                            }
                            else
                            {
                                // TODO why the hell is this commented out

                                // key = initiatingAccount();
                                // Hash256 index = Hash256.accountIDLedgerIndex(key);
                                // if (index.equals(node.get(Hash256.LedgerIndex))) {
                                //     accounts.put(key, node);
                                // }
                            }
                        }
                    }
                }
            }

            return accounts;
        }

        public enum Source
        {
            request_tx_result,
            request_account_tx,
            request_account_tx_binary,
            transaction_subscription_notification
        }
    }
}
