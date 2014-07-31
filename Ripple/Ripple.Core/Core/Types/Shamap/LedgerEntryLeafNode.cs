namespace Ripple.Core.Core.Types.Shamap
{
    using Ripple.Core.Core.Coretypes.Hash;
    using Ripple.Core.Core.Coretypes.Hash.Prefixes;
    using Ripple.Core.Core.Types.Known.Sle;

    public class LedgerEntryLeafNode : ShaMapLeafNode
    {
        private LedgerEntry sle;

        public LedgerEntryLeafNode(LedgerEntry so)
        {
            this.Index = so.Index;
            this.sle = so;
        }

        public override Hash256 Hash()
        {
            var half = new Hash256.HalfSha512();
            half.Update(HashPrefix.LeafNode);
            this.sle.ToBytesSink(half);
            half.Update(this.Index);
            return half.Finish();
        }
    }
}
