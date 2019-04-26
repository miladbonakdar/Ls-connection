using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Actions.Contracts;
using RayanCnc.LSConnection.Events;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Actions.WriteActions
{
    public class WriteBase : LsAction, IWrite
    {
        public Task WriteAsync()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<OnWritedSuccessfullyEventArgs> OnWritedSuccessfully;
    }
}