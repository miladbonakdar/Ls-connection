using System;
using System.Collections.Generic;
using System.Text;
using RayanCnc.LSConnection.Models;

namespace RayanCnc.LSConnection.Events
{
    internal interface ILSEventArgs
    {
        PacketInfo PacketInfo { set; get; }
    }
}