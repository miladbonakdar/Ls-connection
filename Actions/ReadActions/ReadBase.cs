using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Actions.Contracts;
using RayanCnc.LSConnection.Events;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Actions.ReadActions
{
    public class ReadBase : LsAction, IRead
    {
        public ReadBase() : base()
        {
        }

        public Task ReadAsync()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<OnReadedSuccessfullyEventArgs> OnReadedSuccessfully;
    }
}