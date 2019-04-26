using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.WriteActions
{
    public class WriteWord : WriteBase
    {
        public IPacket<ushort> Packet { get; set; }
    }
}