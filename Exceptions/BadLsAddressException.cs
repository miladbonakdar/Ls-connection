using System;

namespace RayanCNC.LSConnection.Exceptions
{
    internal class BadLsAddressException : Exception
    {
        public BadLsAddressException(string message = "The address input is not valid") : base(message)
        {
        }

        public BadLsAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
