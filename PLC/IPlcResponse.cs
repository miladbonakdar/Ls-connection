using System;
using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Packet;

namespace RayanCnc.LSConnection.PLC
{
    public interface IPlcResponse<T> : IHistory
    {
        IPacket<T> Packet { get; set; }
        byte[] RawResponse { set; get; }
        DateTime ResponseOn { set; get; }
        T Value { get; }
    }
}
