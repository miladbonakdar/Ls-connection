namespace RayanCnc.LSConnection.Events
{
    internal interface ILsEventArgs
    {
        object Packet { set; get; }
        object Request { get; set; }
    }
}
