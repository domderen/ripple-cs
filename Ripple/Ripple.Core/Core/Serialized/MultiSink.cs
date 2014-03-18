namespace Ripple.Core.Core.Serialized
{
    public class MultiSink : IBytesSink
    {
        private readonly IBytesSink[] _sinks;

        public MultiSink(params IBytesSink[] sinks)
        {
            _sinks = sinks;
        }

        public void Add(byte aByte)
        {
            foreach (var sink in _sinks)
            {
                sink.Add(aByte);
            }
        }

        public void Add(byte[] bytes)
        {
            foreach (var sink in _sinks)
            {
                sink.Add(bytes);
            }
        }
    }
}
