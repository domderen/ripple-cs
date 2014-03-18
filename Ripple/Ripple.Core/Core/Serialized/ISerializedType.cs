namespace Ripple.Core.Core.Serialized
{
    public interface ISerializedType
    {
        object ToJson();

        byte[] ToBytes();

        string ToHex();

        void ToBytesSink(IBytesSink to);
    }
}
