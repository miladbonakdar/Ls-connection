using System;
using System.Net;

namespace RayanCnc.LSConnection.PLC
{
    public class PlcModel : IPlcModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public IPAddress IP { get; set; } = LsConnectionStatics.DefaultPlcIpAddress;
        public long MemorySize { get; set; } = LsConnectionStatics.DefaultMemorySize;
        public string Name { get; set; } = LsConnectionStatics.DefaultPlcName;
        public byte[] PacketHeader { get; set; } = LsConnectionStatics.DefaultPacketHeader;
        public int PortNumber { get; set; } = LsConnectionStatics.DefaultPlcPortNumber;
    }
}
