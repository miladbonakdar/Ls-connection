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
        public Guid Id { get; set; } = Guid.NewGuid();
        public int PortNumber { get; set; } = LsConnectionStatics.DefaultPlcPortNumber;
        public IPAddress IP { get; set; } = LsConnectionStatics.DefaultPlcIpAddress;
        public string Name { get; set; } = LsConnectionStatics.DefaultPlcName;
        public long MemorySize { get; set; } = LsConnectionStatics.DefaultMemorySize;
        public byte[] PacketHeader { get; set; } = LsConnectionStatics.DefaultPacketHeader;
    }
}