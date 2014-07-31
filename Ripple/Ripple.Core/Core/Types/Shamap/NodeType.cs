namespace Ripple.Core.Core.Types.Shamap
{
    public enum NodeType
    {
        tnERROR,
        tnINNER,
        tnTRANSACTION_NM, // transaction, no metadata
        tnTRANSACTION_MD, // transaction, with metadata
        tnACCOUNT_STATE
    }
}
