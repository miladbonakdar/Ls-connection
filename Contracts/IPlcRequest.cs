using System;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Contracts
{
    public interface IPlcRequest : IHistory
    {
        DateTime StartedOn { set; get; }

        byte[] Data { set; get; }
        string RequestedFrom { set; get; }
        object Packet { get; set; }
    }
}
