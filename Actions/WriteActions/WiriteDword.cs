using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Actions.Contracts;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Actions.WriteActions
{
    public class WiriteDword : WriteBase
    {
        public IPacket<uint> Packet { get; set; }
    }
}