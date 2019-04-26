using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace RayanCNC.LSConnection.Exceptions
{
    internal class OutOfLsMemoryRangeException : Exception
    {
        public OutOfLsMemoryRangeException(string message = "the memory you specified is out of the plc memory range") : base(message)
        {
        }

        public OutOfLsMemoryRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}