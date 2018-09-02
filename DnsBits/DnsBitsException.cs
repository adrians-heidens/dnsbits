using System;

namespace DnsBits
{
    /// <summary>
    /// Base exceptions for BnsBits project.
    /// </summary>
    public class DnsBitsException : Exception
    {
        public DnsBitsException() : base() { }
        
        public DnsBitsException(string message) : base(message) { }

        public DnsBitsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
