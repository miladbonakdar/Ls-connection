using System;
using System.Net;

namespace RayanCnc.LSConnection.Contracts
{
    public interface IPlcModel
    {
        Guid Id { get; }
        int PortNumber { get; }
        IPAddress IP { get; }
        string Name { get; }
        long MemorySize { get; } // in bit
        byte[] PacketHeader { get; }
    }
}