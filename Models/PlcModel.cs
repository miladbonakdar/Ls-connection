using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using RayanCnc.LSConnection;
using RayanCnc.LSConnection.Contracts;

namespace RayanCNC.LSConnection.Models
{
    public class PlcModel : IPlcModel
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public int PortNumber { get; private set; } = LsConnectionStatics.DefaultPlcPortNumber;
        public IPAddress IP { get; private set; } = LsConnectionStatics.DefaultPlcIpAddress;
        public string Name { get; private set; } = LsConnectionStatics.DefaultPlcName;
        public long MemorySize { get; private set; } = LsConnectionStatics.DefaultMemorySize;
        public byte[] PacketHeader { get; private set; } = LsConnectionStatics.DefaultPacketHeader;
    }
}