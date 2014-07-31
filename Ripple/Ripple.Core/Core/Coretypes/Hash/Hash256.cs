using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;
using Ripple.Core.Core.Coretypes.Hash.Prefixes;
using Ripple.Core.Core.Fields;
using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Coretypes.Hash
{
    public class Hash256 : Hash<Hash256>
    {
        public Hash256(byte[] bytes)
            : base(bytes, 32)
        {
        }

        public static OutTranslator OutTranslate = new OutTranslator();
        public static InTranslator InTranslate = new InTranslator();
        public static TypedFields.Hash256Field LedgerHash = new Hash256Field(Field.LedgerHash);
        public static TypedFields.Hash256Field ParentHash = new Hash256Field(Field.ParentHash);
        public static TypedFields.Hash256Field TransactionHash = new Hash256Field(Field.TransactionHash);
        public static TypedFields.Hash256Field AccountHash = new Hash256Field(Field.AccountHash);
        public static TypedFields.Hash256Field PreviousTxnID = new Hash256Field(Field.PreviousTxnID);
        public static TypedFields.Hash256Field AccountTxnID = new Hash256Field(Field.AccountTxnID);
        public static TypedFields.Hash256Field LedgerIndex = new Hash256Field(Field.LedgerIndex);
        public static TypedFields.Hash256Field WalletLocator = new Hash256Field(Field.WalletLocator);
        public static TypedFields.Hash256Field RootIndex = new Hash256Field(Field.RootIndex);
        public static TypedFields.Hash256Field BookDirectory = new Hash256Field(Field.BookDirectory);
        public static TypedFields.Hash256Field InvoiceID = new Hash256Field(Field.InvoiceID);
        public static TypedFields.Hash256Field Nickname = new Hash256Field(Field.Nickname);
        public static TypedFields.Hash256Field Feature = new Hash256Field(Field.Feature);

        public static TypedFields.Hash256Field hash = new Hash256Field(Field.hash);
        public static TypedFields.Hash256Field index = new Hash256Field(Field.index);

        public static Hash256 PrefixedHalfSha512(byte[] prefix, byte[] blob)
        {
            var messageDigest = new HalfSha512();
            messageDigest.Update(prefix);
            messageDigest.Update(blob);
            return messageDigest.Finish();
        }

        public static Hash256 PrefixedHalfSha512(IPrefix prefix, byte[] blob)
        {
            var messageDigest = new HalfSha512();
            messageDigest.Update(prefix);
            messageDigest.Update(blob);
            return messageDigest.Finish();
        }

        public static HalfSha512 Prefixed256(HashPrefix bytes)
        {
            var halfSha512 = new HalfSha512();
            halfSha512.Update(bytes);
            return halfSha512;
        }

        public static Hash256 SigningHash(byte[] blob)
        {
            return PrefixedHalfSha512(HashPrefix.TxSign.Bytes, blob);
        }

        public static Hash256 TransactionId(byte[] blob)
        {
            return PrefixedHalfSha512(HashPrefix.TransactionId, blob);
        }

        public static Hash256 AccountIdLedgerIndex(AccountId accountId)
        {
            return PrefixedHalfSha512(LedgerSpace.Account, accountId.Bytes);
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

        public int Nibblet(int depth)
        {
            int byteIx = depth > 0 ? depth / 2 : 0;
            int b = HashBytes[byteIx];
            if (depth % 2 == 0)
            {
                b = (b & 0xF0) >> 4;
            }
            else
            {
                b = b & 0x0F;
            }

            return b;
        }

        public class Hash256Field : TypedFields.Hash256Field
        {
            private readonly Field _f;

            public Hash256Field(Field f)
            {
                _f = f;
            }

            public override Field GetField()
            {
                return _f;
            }
        }

        public class OutTranslator : OutHashTranslator<Hash256>
        {
            public override Hash256 NewInstance(byte[] b)
            {
                return new Hash256(b);
            }

            public override int ByteWidth()
            {
                return 32;
            }
        }

        public class InTranslator : InHashTranslator<Hash256>
        {
        }

        public class Hash256Map<TValue> : SortedDictionary<Hash256, TValue>
        { }

        public class HalfSha512 : IBytesSink
        {
            private readonly Sha512Digest _messageDigest;

            public HalfSha512()
            {
                _messageDigest = new Sha512Digest();
            }

            public Sha512Digest MessageDigest
            {
                get { return _messageDigest; }
            }

            public void Update(byte[] bytes)
            {
                _messageDigest.BlockUpdate(bytes, 0, bytes.Length);
            }

            public void Update(Hash256 hash)
            {
                _messageDigest.BlockUpdate(hash.Bytes, 0, hash.Bytes.Length);
            }

            public Hash256 Finish()
            {
                var digest = new byte[64];

                _messageDigest.DoFinal(digest, 0);
                var half = new byte[32];
                Array.Copy(digest, 0, half, 0, 32);

                return new Hash256(half);
            }

            public void Add(byte aByte)
            {
                _messageDigest.Update(aByte);
            }

            public void Add(byte[] bytes)
            {
                _messageDigest.BlockUpdate(bytes, 0, bytes.Length);
            }

            public void Update(IPrefix prefix)
            {
                _messageDigest.BlockUpdate(prefix.Bytes, 0, prefix.Bytes.Length);
            }
        }
    }
}
