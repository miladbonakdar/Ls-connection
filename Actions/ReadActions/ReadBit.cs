using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.ReadActions
{
    public class ReadBit : ReadBase
    {
        public IPacket<bool> Packet { get; set; }
    }
}