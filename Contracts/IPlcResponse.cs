using System;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Contracts
{
    public interface IPlcResponse : IHistory
    {
        DateTime ResponsedOn { set; get; }
        byte[] RawResponse { set; get; }

        object Packet { get; set; }
        PacketInfo PacketInfo { get; set; }
    }
}