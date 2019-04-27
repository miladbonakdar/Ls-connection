using System;
using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Events
{
    public class OnConnectEventArgs : ILSEventArgs, IHistory
    {
        public object Packet { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
