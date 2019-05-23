using System;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Extensions
{
    public class LsDataTypeConflictException : Exception
    {
        public LsDataTypeConflictException(LsDataType packetDataType, LsDataType addressDataType)
        : base($"conflict in : packet data type is {packetDataType.ToString()} and the address data type is {addressDataType.ToString()}")
        {
        }
    }
}
