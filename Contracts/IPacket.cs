using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCnc.LSConnection.Contracts
{
    public interface IPacket<T> : IHistory
    {
        Guid Id { set; get; }
        T Value { set; get; }
        ILsAddress Address { set; get; }
        LsDataType DataTpe { set; get; }
        ushort Order { set; get; }
        ActionType ActionType { set; get; }
        byte[] RequestPacketHeader { set; get; }
        byte[] RequestPacketInformation { set; get; }
        byte[] RequestPackeInstruction { set; get; }
        byte[] RawData { get; }
        byte[] RawResponse { get; }

        void SetRawValue(byte[] raw);
    }
}
