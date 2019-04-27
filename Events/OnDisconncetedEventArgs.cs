using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Events
{
    public class OnDisconncetedEventArgs : ILSEventArgs, IHistory
    {
        public object Packet { get; set; }
        public DateTime CreatedOn { set; get; }
    }
}
