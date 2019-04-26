using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.ReadActions
{
    public class ReadByte : ReadBase
    {
        public IPacket<byte> Packet { get; set; }
    }
}