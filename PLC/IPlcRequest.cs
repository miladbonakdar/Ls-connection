using System;
using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Packet;

namespace RayanCnc.LSConnection.PLC
{
    public interface IPlcRequest<T> : IHistory
    {
        byte[] Data { set; get; }
        IPacket<T> Packet { get; set; }
        DateTime StartedOn { set; get; }
    }
}
