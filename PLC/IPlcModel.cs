using System;
using System.Net;

namespace RayanCnc.LSConnection.PLC
{
    public interface IPlcModel
    {
        Guid Id { get; }
        IPAddress IP { get; }
        long MemorySize { get; }
        string Name { get; }

        // in bit
        byte[] PacketHeader { get; }

        int PortNumber { get; }
    }
}
