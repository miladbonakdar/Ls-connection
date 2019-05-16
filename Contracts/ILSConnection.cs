using RayanCnc.LSConnection.Events;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using RayanCnc.LSConnection.Packet;
using RayanCnc.LSConnection.PLC;

namespace RayanCnc.LSConnection.Contracts
{
    public interface ILsConnection
    {
        event EventHandler<OnConnectEventArgs> OnConnect;

        event EventHandler<OnDisconnectEventArgs> OnDisconnected;

        event EventHandler<OnReadedSuccessfullyEventArgs> OnReadSuccessfully;

        event EventHandler<OnWriteSuccessfullyEventArgs> OnWriteSuccessfully;

        TcpClient Client { set; get; }
        bool Connected { set; get; }
        Stream NetworkStream { set; get; }
        IPlcModel PlcModel { set; get; }

        Task<ILsConnection> ConnectAsync();

        void Disconnect();

        void MakeEmpty();

        Task<IPlcResponse<T>> ReadAsync<T>(IPacket<T> packet);

        Task<T> ReadAsync<T>(ILsAddress address);

        Task<T> ReadAsync<T>(string address);

        Task<IPlcResponse<T>> ReadResponseAsync<T>(string address);

        Task<IPlcResponse<T>> ReadResponseAsync<T>(ILsAddress address);

        Task<IPlcResponse<T>> SendMessageAsync<T>(IPlcRequest<T> request);

        Task<IPlcResponse<T>> WriteAsync<T>(IPacket<T> packet);

        Task<T> WriteAsync<T>(ILsAddress address, T value);

        Task<T> WriteAsync<T>(string address, T value);

        Task<IPlcResponse<T>> WriteResponseAsync<T>(string address, T value);

        Task<IPlcResponse<T>> WriteResponseAsync<T>(ILsAddress address, T value);
    }
}
