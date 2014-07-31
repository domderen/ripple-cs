namespace Ripple.Core.Core.Coretypes.Hash.Prefixes
{
    using System.Collections.Generic;
    using Ripple.Core.Core.Coretypes.UInt;

    public class HashPrefix : IPrefix
    {
        private readonly byte[] _bytes;

        private UInt32 uInt32;

        private HashPrefix(long i)
        {
            this.uInt32 = new UInt32(i);
            _bytes = this.uInt32.ToByteArray();
        }

        /// <summary>
        /// TXN
        /// </summary>
        public static HashPrefix TransactionId = new HashPrefix(0x54584E00);

        /// <summary>
        /// TND
        /// Transaction plus metadata.
        /// </summary>
        public static HashPrefix TxNode = new HashPrefix(0x534E4400);

        /// <summary>
        /// MLN
        /// Account state.
        /// </summary>
        public static HashPrefix LeafNode = new HashPrefix(0x4D4C4E00);

        /// <summary>
        /// MIN
        /// Inner node in tree.
        /// </summary>
        public static HashPrefix InnerNode = new HashPrefix(0x4D494E00);

        /// <summary>
        /// LGR
        /// Ledger master data for signing.
        /// </summary>
        public static HashPrefix LedgerMaster = new HashPrefix(0x4C575200);

        /// <summary>
        /// STX
        /// Inner transaction to sign.
        /// </summary>
        public static HashPrefix TxSign = new HashPrefix(0x53545800);

        /// <summary>
        /// VAL
        /// Validation for signing.
        /// </summary>
        public static HashPrefix Validation = new HashPrefix(0x56414C00);

        /// <summary>
        /// PRP
        /// Proposal for signing.
        /// </summary>
        public static HashPrefix Proposal = new HashPrefix(0x50525000);

        public static IEnumerable<HashPrefix> Values
        {
            get
            {
                yield return TransactionId;
                yield return TxNode;
                yield return LeafNode;
                yield return InnerNode;
                yield return LedgerMaster;
                yield return TxSign;
                yield return Validation;
                yield return Proposal;
            }
        }

        public byte[] Bytes
        {
            get { return this._bytes; }
        }

        public UInt32 UInt32
        {
            get
            {
                return this.uInt32;
            }
        }
    }
}
