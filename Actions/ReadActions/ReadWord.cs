using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Actions.WriteActions;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.ReadActions
{
    public class ReadWord : ReadBase
    {
        public IPacket<ushort> Packet { get; set; }
    }
}