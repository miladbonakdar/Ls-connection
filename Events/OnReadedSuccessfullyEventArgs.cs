using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Events
{
    public class OnReadedSuccessfullyEventArgs : ILSEventArgs
    {
        public PacketInfo PacketInfo { get; set; }
    }
}