using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Events
{
    public class OnReadedSuccessfullyEventArgs : ILSEventArgs
    {
        public object Packet { get; set; }
    }
}
