using System;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Actions.Contracts;
using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Actions
{
    public abstract class LsAction : ILsAction, IHistory
    {
        public LsAction(ILSConnection connection = null)
        {
            Connection = connection ?? LSConnection.Connection;
            CreatedOn = DateTime.Now;
        }

        public static long ActionIdCounter { set; get; } = 0;

        public ActionStatus ActionStatus { get; set; }

        public byte[] PacketArray { get; set; }

        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public ILSConnection Connection { get; set; }

        public virtual Task Run()
        {
            StartedOn = DateTime.Now;
            throw new NotImplementedException();
        }

        public virtual Task CreatePacketHeader()
        {
            throw new NotImplementedException();
        }

        public virtual Task CreatePacket()
        {
            throw new NotImplementedException();
        }
    }
}