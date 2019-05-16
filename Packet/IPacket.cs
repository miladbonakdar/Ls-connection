using System;
using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.DataTypeStrategy;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCnc.LSConnection.Packet
{
    public interface IPacket<out T> : IHistory
    {
        ActionType ActionType { get; }
        ILsAddress Address { get; }
        LsDataType DataType { get; }
        ITypeStrategy DataTypeStrategy { get; }
        Guid Id { get; }
        ushort Order { get; }
        byte[] RawRequest { get; }
        byte[] RawResponse { get; }
        byte[] RequestPacketHeader { get; }
        byte[] RequestPacketInformation { get; }
        byte[] RequestPacketInstruction { get; }
        DateTime ResponseOn { get; }
        T Value { get; }

        void ParseRawBytes(byte[] raw);
    }
}
