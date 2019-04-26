using System;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCNC.LSConnection.Contracts
{
    public interface IPacketInfo
    {
        Guid PacketId { set; get; }
        ILsAddress Address { set; get; }
        LsDataType DataTpe { set; get; }
        ushort Order { set; get; }
        object Packet { get; set; }
    }
}