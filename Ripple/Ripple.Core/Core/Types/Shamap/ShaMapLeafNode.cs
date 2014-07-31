namespace Ripple.Core.Core.Types.Shamap
{
    using System;

    using Ripple.Core.Core.Coretypes.Hash;
    using Ripple.Core.Core.Coretypes.Hash.Prefixes;

    public class ShaMapLeafNode : ShaMapNode
    {
        private Hash256 index;

        public ShaMapLeafNode()
        {
        }

        public ShaMapLeafNode(Hash256 index, NodeType type, IItem blob)
        {
            this.index = index;
            this.Type = type;
            this.Blob = blob;
        }

        public IItem Blob { get; set; }

        public Hash256 Index
        {
            get
            {
                return this.index;
            }

            protected set
            {
                this.index = value;
            }
        }

        public override Hash256 Hash()
        {
            var half = new Hash256.HalfSha512();
            HashPrefix prefix;

            switch (this.Type)
            {
                case NodeType.tnTRANSACTION_MD:
                    prefix = HashPrefix.TxNode;
                    break;
                case NodeType.tnACCOUNT_STATE:
                    prefix = HashPrefix.LeafNode;
                    break;
                default:
                    throw new InvalidOperationException("No support for " + this.Type);
            }

            half.Update(prefix.Bytes);
            half.Update(this.Blob.Bytes);
            half.Update(this.index);

            return half.Finish();
        }
    }
}
