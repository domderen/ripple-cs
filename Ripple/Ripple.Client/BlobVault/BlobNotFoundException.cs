using System;

namespace Ripple.Client.BlobVault
{
    public class BlobNotFoundException : ApplicationException
    {
        public BlobNotFoundException(string message)
            : base(message)
        {
        }
    }
}
