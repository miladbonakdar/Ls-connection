using System;
using RayanCnc.LSConnection.Packet;

namespace RayanCnc.LSConnection.PLC
{
    public class PlcRequest<T> : IPlcRequest<T>
    {
        public DateTime CreatedOn { get; set; }
        public byte[] Data { get; set; }
        public IPacket<T> Packet { get; set; }
        public DateTime StartedOn { get; set; }
    }
}
