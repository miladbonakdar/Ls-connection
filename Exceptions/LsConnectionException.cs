using System;
using System.Collections.Generic;
using System.Text;

namespace RayanCNC.LSConnection.Exceptions
{
    public class LsConnectionException : Exception
    {
        public LsConnectionException(string message) : base(message)
        {
        }

        public LsConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}