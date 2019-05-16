using RayanCnc.LSConnection.Contracts;
using System;

namespace RayanCnc.LSConnection.Events
{
    public class OnDisconnectEventArgs : IHistory
    {
        public DateTime CreatedOn { set; get; }
    }
}
