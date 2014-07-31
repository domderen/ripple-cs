namespace Ripple.Core.Core.Types.Shamap
{
    using System;

    using Ripple.Core.Core.Coretypes.Hash;
    using Ripple.Core.Core.Coretypes.Hash.Prefixes;
    using Ripple.Core.Core.Types.Known.Sle;

    public class ShaMapInnerNode : ShaMapNode
    {
        public static Hash256 ZERO_256 = new Hash256(new byte[32]);

        public ShaMapNode[] branches;

        protected int slotBits = 0;

        public int depth;

        public Hash256 hash;

        protected ShaMapInnerNode(int node_depth)
        {
            branches = new ShaMapNode[16];
            Type = NodeType.tnINNER;
            HashingPrefix = HashPrefix.InnerNode;
            depth = node_depth;
        }

        public bool Empty
        {
            get
            {
                return slotBits == 0;
            }
        }

        protected bool NeedsHashing
        {
            get
            {
                return this.hash == null;
            }
        }

        public override Hash256 Hash()
        {
            if (!this.NeedsHashing)
            {
                return this.hash;
            }

            if (this.Empty)
            {
                return ZERO_256;
            }

            var hasher = new Hash256.HalfSha512();
            hasher.Update(this.HashingPrefix);

            int fullBranches = 0;
            foreach (var node in this.branches)
            {
                if (node != null)
                {
                    var hsh = node.Hash();
                    hasher.Update(hsh);
                    fullBranches++;
                } 
                else
                {
                    hasher.Update(ZERO_256);
                }
            }

            this.hash = hasher.Finish();
            this.OnHash(this.hash, fullBranches);
            return this.hash;
        }

        public void OnHash(Hash256 hash, int fullBranches)
        {
        }

        public ShaMapLeafNode GetLeafForUpdating(Hash256 id)
        {
            return this.GetLeaf(id, true);
        }

        public ShaMapLeafNode GetLeaf(Hash256 id)
        {
            return this.GetLeaf(id, false);
        }
        public ShaMapInnerNode MakeInnerChild()
        {
            return new ShaMapInnerNode(this.depth + 1);
        }

        public void AddSLE(LedgerEntry sle)
        {
            this.AddLeaf(sle.Index, new LedgerEntryLeafNode(sle));
        }

        public void AddItem(Hash256 index, NodeType nodeType, IItem blob)
        {
            this.AddLeaf(index, new ShaMapLeafNode(index, nodeType, blob));
        }

        protected void SetNode(int slot, ShaMapNode node)
        {
            this.slotBits = this.slotBits | (1 << slot);
            this.branches[slot] = node;
        }

        protected void Invalidate()
        {
            this.hash = null;
        }

        protected void RemoveNode(int slot)
        {
            this.branches[slot] = null;
            this.slotBits = this.slotBits & ~(1 << slot);
        }

        protected ShaMapLeafNode GetLeaf(Hash256 id, bool invalidating) 
        {
            int ix = id.Nibblet(this.depth);
            ShaMapNode existing = this.branches[ix];
            if (invalidating) 
            {
                this.Invalidate();
            }

            if (existing == null)
            {
                return null;
            }

            var node = existing as ShaMapLeafNode;
            if (node != null) 
            {
                return node;
            }

            return ((ShaMapInnerNode)existing).GetLeaf(id, invalidating);
        }

        private void AddLeaf(Hash256 index, ShaMapLeafNode leaf)
        {
            int ix = index.Nibblet(this.depth);
            var existing = this.branches[ix];
            this.Invalidate();

            if (existing == null)
            {
                this.SetNode(ix, leaf);
            }
            else
            {
                var node = existing as ShaMapLeafNode;
                if (node != null)
                {
                    if (node.Index.Equals(index))
                    {
                        throw new InvalidOperationException("Tried to add node already in tree!");
                    }

                    var container = this.MakeInnerChild();
                    container.AddLeaf(node.Index, node);
                    container.AddLeaf(index, leaf);
                    this.SetNode(ix, container);
                }
                else
                {
                    var existingInner = (ShaMapInnerNode)existing;
                    existingInner.AddLeaf(index, leaf);
                }
            }
        }
    }
}
