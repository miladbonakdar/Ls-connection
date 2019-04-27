using System;
using System.Collections.Generic;
using System.Text;

namespace RayanCnc.LSConnection.Models
{
    public enum PacketType
    {
        ReadBit, ReadByte, ReadWord, ReadDWord, ReadContinuous,
        WriteBit, WriteByte, WriteWord, WriteDWord, WriteContinuous
    }
}