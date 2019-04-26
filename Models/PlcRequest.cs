using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Models
{
    public class PlcRequest : IPlcRequest
    {
        public DateTime CreatedOn { get; set; }
        public DateTime StartedOn { get; set; }
        public byte[] Data { get; set; }
        public string RequestedFrom { get; set; }
        public object Packet { get; set; }
        public PacketInfo PacketInfo { get; set; }
    }
}