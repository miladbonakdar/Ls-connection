using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Events;
using RayanCnc.LSConnection.Models;
using RayanCnc.LSConnection.Packet;
using RayanCnc.LSConnection.PLC;
using RayanCNC.LSConnection.Exceptions;
using RayanCNC.LSConnection.Extensions;
using RayanCNC.LSConnection.LsAddress.Contracts;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RayanCnc.LSConnection
{
    public class LsConnection : ILsConnection, IPing, IDisposable
    {
        private bool _connected;

        public LsConnection(IPlcModel plcModel = null)
        {
            Connection = this;
            PlcModel = plcModel ?? DefaultPlcModel;
        }

        public event EventHandler<OnConnectEventArgs> OnConnect;

        public event EventHandler<OnDisconnectEventArgs> OnDisconnected;

        public event EventHandler<OnReadedSuccessfullyEventArgs> OnReadSuccessfully;

        public event EventHandler<OnWriteSuccessfullyEventArgs> OnWriteSuccessfully;

        public static ILsConnection Connection { get; private set; }
        public static IPlcModel DefaultPlcModel { get; private set; } = new PlcModel();
        public TcpClient Client { get; private set; }

        public bool Connected
        {
            get => _connected;
            protected set
            {
                if (_connected == value) return;
                _connected = value;
                if (value)
                    OnConnect?.Invoke(this, null);
                else
                    OnDisconnected?.Invoke(this, null);
            }
        }

        public Stream NetworkStream { get; private set; }
        public IPlcModel PlcModel { get; protected set; }

        public static void SetDefaultPlcModel(IPlcModel plcModel) => DefaultPlcModel = plcModel;

        //https://msdn.microsoft.com/en-us/magazine/dn605876.aspx
        public async Task<ILsConnection> ConnectAsync()
        {
            if (!await PingAsync())
                throw new LsConnectionException("We cannot find the server . please check the Ip first");
            try
            {
                Client = new TcpClient();
                await Client.ConnectAsync(PlcModel.IP, PlcModel.PortNumber);
                NetworkStream = Client.GetStream();
                Connected = true;
            }
            catch (Exception ex)
            {
                throw new LsConnectionException($"LsConnection connect error : {ex.Message}", ex);
            }
            return this;
        }

        public void Disconnect()
        {
            MakeEmpty();
            Connected = false;
        }

        public void Dispose()
        {
            NetworkStream?.Dispose();
            Client?.Dispose();
            Disconnect();
        }

        public void MakeEmpty()
            => (NetworkStream, Client) = (null, null);

        public async Task<bool> PingAsync()
        {
            Ping ping = new Ping();
            try
            {
                if (PlcModel.IP == null) throw new LsPingException("Ip is not set yet");
                PingReply reply = await ping.SendPingAsync(PlcModel.IP, 500,
                    Encoding.ASCII.GetBytes("I am a programmer. I am a window. I am a book."), new PingOptions(64, true));
                return reply != null && reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                Disconnect();
                return false;
            }
            catch (Exception e) { throw new LsPingException(e.Message, e); }
        }

        public async Task<IPlcResponse<T>> ReadAsync<T>(IPacket<T> packet) => await SendMessageAsync<T>(new PlcRequest<T>
        {
            Packet = packet,
            CreatedOn = DateTime.Now,
            StartedOn = DateTime.Now,
            Data = packet.RawRequest
        });

        public async Task<T> ReadAsync<T>(ILsAddress address) =>
            (await ReadAsync<T>(new Packet<T>(address, PlcModel.PacketHeader))).Value;

        public async Task<T> ReadAsync<T>(string address) =>
            (await ReadAsync<T>(new Packet<T>(address, PlcModel.PacketHeader))).Value;

        public async Task<IPlcResponse<T>> ReadResponseAsync<T>(ILsAddress address) =>
            await ReadAsync<T>(new Packet<T>(address, PlcModel.PacketHeader));

        public async Task<IPlcResponse<T>> ReadResponseAsync<T>(string address) =>
            await ReadAsync<T>(new Packet<T>(address, PlcModel.PacketHeader));

        public async Task<IPlcResponse<T>> SendMessageAsync<T>(IPlcRequest<T> request)
        {
            if (!Connected)
                throw new InvalidOperationException("the connection is not connected yet");

            await NetworkStream.WriteAsync(request.Data, 0, request.Data.Length);
            byte[] response = new byte[LsConnectionStatics.MaxPlcResponseLength];
            var bytesCount = await NetworkStream.ReadAsync(response, 0, response.Length);
            if (bytesCount == 0) { Disconnect(); return null; }
            var raw = response.SubArray(0, bytesCount);
            HandleRawResponse(request, raw);
            return new PlcResponse<T>
            {
                RawResponse = raw,
                CreatedOn = DateTime.Now,
                ResponseOn = DateTime.Now,
                Packet = request.Packet
            };
        }

        public async Task<IPlcResponse<T>> WriteAsync<T>(IPacket<T> packet) => await SendMessageAsync<T>(new PlcRequest<T>
        {
            Packet = packet,
            CreatedOn = DateTime.Now,
            StartedOn = DateTime.Now,
            Data = packet.RawRequest
        });

        public async Task<T> WriteAsync<T>(ILsAddress address, T value) =>
            (await WriteAsync<T>(new Packet<T>(value, address, PlcModel.PacketHeader))).Value;

        public async Task<T> WriteAsync<T>(string address, T value) =>
            (await WriteAsync(new Packet<T>(value, address, PlcModel.PacketHeader))).Value;

        public async Task<IPlcResponse<T>> WriteResponseAsync<T>(ILsAddress address, T value) =>
                            await WriteAsync(new Packet<T>(value, address, PlcModel.PacketHeader));

        public async Task<IPlcResponse<T>> WriteResponseAsync<T>(string address, T value) =>
            await WriteAsync(new Packet<T>(value, address, PlcModel.PacketHeader));

        private void HandleRawResponse<T>(IPlcRequest<T> request, byte[] raw)
        {
            request.Packet.ParseRawBytes(raw);
            switch (request.Packet.ActionType)
            {
                case ActionType.Read:
                    OnReadSuccessfully?.Invoke(this, new OnReadedSuccessfullyEventArgs { Packet = request.Packet });
                    break;

                default:
                    OnWriteSuccessfully?.Invoke(this, new OnWriteSuccessfullyEventArgs { Packet = request.Packet });
                    break;
            }
        }
    }
}
