using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.ReadActions
{
    public class ReadDWord : ReadBase
    {
        public IPacket<uint> Packet { get; set; }
    }
}