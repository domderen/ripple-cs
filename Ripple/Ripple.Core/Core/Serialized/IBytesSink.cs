namespace Ripple.Core.Core.Serialized
{
    public interface IBytesSink
    {
        void Add(byte aByte);

        void Add(byte[] bytes);
    }
}
