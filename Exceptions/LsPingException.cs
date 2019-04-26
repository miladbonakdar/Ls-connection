using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace RayanCNC.LSConnection.Exceptions
{
    internal class LsPingException : Exception
    {
        public LsPingException(string message) : base(message)
        {
        }

        public LsPingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LsPingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}