using Ripple.Core.Core.Serialized;

namespace Ripple.Core.Core.Binary
{
    public class BinaryWriter
    {
        private readonly BytesList _list;

        public BinaryWriter(BytesList list)
        {
            _list = list;
        }

        public void Write(ISerializedType obj)
        {
            obj.ToBytesSink(_list);
        }
    }
}
