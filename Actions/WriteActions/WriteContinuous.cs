using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.WriteActions
{
    public class WriteContinuous : WriteBase
    {
        public IPacket<byte[]> Packet { get; set; }
    }
}