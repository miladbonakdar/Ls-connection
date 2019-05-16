using System;
using RayanCnc.LSConnection.Packet;

namespace RayanCnc.LSConnection.PLC
{
    public class PlcResponse<T> : IPlcResponse<T>
    {
        public DateTime CreatedOn { get; set; }
        public IPacket<T> Packet { get; set; }
        public byte[] RawResponse { get; set; }
        public DateTime ResponseOn { get; set; }
        public T Value => Packet.Value;
    }
}
