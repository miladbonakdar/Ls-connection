using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Events;
using RayanCNC.LSConnection.LsAddress.Contracts;

namespace RayanCnc.LSConnection.Contracts
{
    public interface ILSConnection
    {
        bool Connected { set; get; }
        TcpClient Client { set; get; }
        IPlcModel PlcModel { set; get; }

        Stream NetworkStream { set; get; }

        event EventHandler<OnDisconncetedEventArgs> OnDisconnceted;

        event EventHandler<OnConnectEventArgs> OnConnect;

        event EventHandler<OnReadedSuccessfullyEventArgs> OnReadedSuccessfully;

        event EventHandler<OnWritedSuccessfullyEventArgs> OnWritedSuccessfully;

        Task<ILSConnection> ConnectAsync();

        void Disconnect();

        void MakeEmpty();

        void SetDefaultPlcModel(IPlcModel plcModel);

        Task<IPlcResponse> SendMessageAsync(IPlcRequest request);

        Task<IPlcResponse> ReadAsync<T>(IPacket<T> packet);

        Task<IPlcResponse> WriteAsync<T>(IPacket<T> packet);

        Task<IPlcResponse> ReadAsync<T>(ILsAddress address);

        Task<IPlcResponse> WriteAsync<T>(ILsAddress address, T value);
    }
}