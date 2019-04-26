using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Events;

namespace RayanCnc.LSConnection.Actions.Contracts
{
    public interface IRead : ILsAction
    {
        Task ReadAsync();

        event EventHandler<OnReadedSuccessfullyEventArgs> OnReadedSuccessfully;
    }
}