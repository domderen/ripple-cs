using System;

namespace Ripple.Core.Core.Coretypes
{
    /// <summary>
    /// Thrown when an Amount is constructed with an invalid value.
    /// </summary>
    public class PrecisionException : ApplicationException
    {
        public PrecisionException(string message)
            : base(message)
        {
        }
    }
}
