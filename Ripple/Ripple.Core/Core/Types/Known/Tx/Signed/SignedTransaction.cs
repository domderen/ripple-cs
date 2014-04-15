using System;
using Ripple.Core.Core.Coretypes;
using Ripple.Core.Core.Coretypes.Hash;
using Ripple.Core.Core.Coretypes.Hash.Prefixes;
using Ripple.Core.Core.Enums;
using Ripple.Core.Core.Serialized;
using Ripple.Core.Crypto.Ecdsa;
using UInt32 = Ripple.Core.Core.Coretypes.UInt.UInt32;

namespace Ripple.Core.Core.Types.Known.Tx.Signed
{
    public class SignedTransaction
    {
        public Transaction Txn;
        public Hash256 Hash;
        public Hash256 SigningHash;
        public Hash256 PreviousSigningHash;
        public string TxBlob;

        public TransactionType TransactionType
        {
            get { return Txn.TransactionType(); }
        }

        public void Prepare(IKeyPair keyPair, Amount fee, UInt32 sequence, UInt32 lastLedgerSequence)
        {
            // This won't always be specified
            if (lastLedgerSequence != null)
            {
                Txn.Add(UInt32.LastLedgerSequence, lastLedgerSequence);
            }

            Txn.Add(UInt32.Sequence, sequence);
            Txn.Add(Amount.Fee, fee);
            Txn.Add(VariableLength.SigningPubKey, keyPair.PubBytes());

            if (Transaction.CanonicalFlagDeployed)
            {
                Txn.SetCanonicalSignatureFlag();
            }

            SigningHash = Txn.SigningHash();
            if (PreviousSigningHash != null && SigningHash.Equals(PreviousSigningHash))
            {
                return;
            }

            try
            {
                byte[] signature = keyPair.Sign(SigningHash.Bytes);
                Txn.Add(VariableLength.TxnSignature, signature);

                var blob = new BytesList();
                Hash256.HalfSha512 id = Hash256.Prefixed256(HashPrefix.TransactionId);

                Txn.ToBytesSink(new MultiSink(blob, id));
                TxBlob = blob.BytesHex();
                Hash = id.Finish();
            }
            catch (Exception e)
            {
                // electric paranoia
                PreviousSigningHash = null;
                throw new ApplicationException("Something went wrong.", e);
            }

            PreviousSigningHash = SigningHash;
        }
    }
}
