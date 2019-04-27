using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;

namespace RayanCnc.LSConnection.Models
{
    public class PlcResponse : IPlcResponse
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ResponsedOn { get; set; }
        public byte[] RawResponse { get; set; }
        public object Packet { get; set; }
    }
}
