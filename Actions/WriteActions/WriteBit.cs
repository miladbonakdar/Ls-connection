using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.WriteActions
{
    public class WriteBit : WriteBase
    {
        public IPacket<bool> Packet { get; set; }
    }
}