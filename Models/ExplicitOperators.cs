using System;

namespace RayanCnc.LSConnection.Models
{
    public partial class PacketInfo
    {
        public static explicit operator PacketInfo(Packet<bool> packet) => new PacketInfo
        {
            DataTpe = packet.DataTpe,
            PacketId = packet.Id,
            Order = packet.Order,
            Address = packet.Address,
            Packet = packet
        };

        public static explicit operator PacketInfo(Packet<byte> packet) => new PacketInfo
        {
            DataTpe = packet.DataTpe,
            Address = packet.Address,
            PacketId = packet.Id,
            Order = packet.Order,
            Packet = packet
        };

        public static explicit operator PacketInfo(Packet<ushort> packet) => new PacketInfo
        {
            DataTpe = packet.DataTpe,
            Address = packet.Address,
            PacketId = packet.Id,
            Order = packet.Order,
            Packet = packet
        };

        public static explicit operator PacketInfo(Packet<int> packet) => new PacketInfo
        {
            DataTpe = packet.DataTpe,
            Address = packet.Address,
            PacketId = packet.Id,
            Order = packet.Order,
            Packet = packet
        };

        public static explicit operator PacketInfo(Packet<byte[]> packet) => new PacketInfo
        {
            DataTpe = packet.DataTpe,
            Address = packet.Address,
            PacketId = packet.Id,
            Order = packet.Order,
            Packet = packet
        };
    }
}