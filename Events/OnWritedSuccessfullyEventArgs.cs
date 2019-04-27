using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Events
{
    public class OnWritedSuccessfullyEventArgs : ILSEventArgs
    {
        public object Packet { get; set; }
    }
}
