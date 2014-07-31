namespace Ripple.Core.Core.Types.Shamap
{
    using Ripple.Core.Core.Coretypes.Hash;
    using Ripple.Core.Core.Coretypes.Hash.Prefixes;

    public abstract class ShaMapNode
    {
        protected ShaMapNode()
        {
        }

        public NodeType Type { get; set; }

        public IPrefix HashingPrefix { get; set; }

        public abstract Hash256 Hash();
    }
}
