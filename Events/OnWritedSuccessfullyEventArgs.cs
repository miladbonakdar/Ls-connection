using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Events
{
    public class OnWritedSuccessfullyEventArgs : ILSEventArgs
    {
        public PacketInfo PacketInfo { get; set; }
    }
}