using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.WriteActions
{
    public class WriteByte : WriteBase
    {
        public IPacket<byte> Packet { get; set; }
    }
}