using System;

namespace Ripple.Core.Encodings.Base58
{
    public class EncodingFormatException : ApplicationException
    {
        public EncodingFormatException(string message)
            : base(message)
        {
        }
    }
}
