using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;

namespace RayanCNC.LSConnection.Contracts
{
    public interface IPacketInfo
    {
        ILsAddress Address { set; get; }
        LsDataType DataTpe { set; get; }
        ushort Order { set; get; }
        object Packet { get; set; }
        Guid PacketId { set; get; }
    }
}
