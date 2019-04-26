using System;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Actions.Contracts
{
    public interface ILsAction
    {
        Task Run();

        Task CreatePacketHeader();

        Task CreatePacket();

        ActionStatus ActionStatus { set; get; }
        byte[] PacketArray { set; get; }
        Guid Id { set; get; }
        DateTime CreatedOn { set; get; }
        DateTime StartedOn { set; get; }
        DateTime FinishedOn { set; get; }
        ILSConnection Connection { set; get; }
    }
}