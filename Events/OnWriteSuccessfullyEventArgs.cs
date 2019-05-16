namespace RayanCnc.LSConnection.Events
{
    public class OnWriteSuccessfullyEventArgs : ILsEventArgs
    {
        public object Packet { get; set; }
        public object Request { get; set; }
    }
}
