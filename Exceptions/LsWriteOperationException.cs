using System;

namespace RayanCNC.LSConnection.Exceptions
{
    internal class LsWriteOperationException : Exception
    {
        public LsWriteOperationException(string message = "The write operation result is not acceptable") : base(message)
        {
        }

        public LsWriteOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
