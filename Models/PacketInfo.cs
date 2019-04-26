using System;
using RayanCnc.LSConnection.Contracts;
using RayanCNC.LSConnection.Contracts;
using RayanCNC.LSConnection.LsAddress;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCnc.LSConnection.Models
{
    public partial class PacketInfo : IPacketInfo
    {
        public Guid PacketId { set; get; }
        public ILsAddress Address { get; set; }
        public LsDataType DataTpe { set; get; }
        public ushort Order { set; get; }
        public object Packet { get; set; }
    }
}