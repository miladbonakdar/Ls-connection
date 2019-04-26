using RayanCnc.LSConnection.Contracts;
using RayanCnc.LSConnection.Events;
using RayanCnc.LSConnection.Models;
using RayanCNC.LSConnection.Exceptions;
using RayanCNC.LSConnection.Extentions;
using RayanCNC.LSConnection.LsAddress.Contracts;
using RayanCNC.LSConnection.Models;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RayanCnc.LSConnection
{
    public class LSConnection : ILSConnection, IPing
    {
        public static ILSConnection Connection { get; private set; }//!DONE!//
        public static IPlcModel DefaultPlcModel { get; private set; } = new PlcModel();//!DONE!//
        private bool _connected;//!DONE!//
        public Stream NetworkStream { get; set; }//!DONE!//

        public bool Connected//!DONE!//
        {
            get { return _connected; }
            set
            {
                _connected = value;
                if (value)
                    OnConnect?.Invoke(this, null);
                else
                    OnDisconnceted?.Invoke(this, null);
            }
        }

        public IPlcModel PlcModel { get; set; }//!DONE!//

        public TcpClient Client { get; set; }//!DONE!//

        public event EventHandler<OnDisconncetedEventArgs> OnDisconnceted;//!DONE!//

        public event EventHandler<OnConnectEventArgs> OnConnect;//!DONE!//

        public event EventHandler<OnReadedSuccessfullyEventArgs> OnReadedSuccessfully;

        public event EventHandler<OnWritedSuccessfullyEventArgs> OnWritedSuccessfully;

        public LSConnection(IPlcModel plcModel = null)
        {
            Connection = this;
            PlcModel = plcModel ?? DefaultPlcModel;
            Client = new TcpClient();
        }

        //https://msdn.microsoft.com/en-us/magazine/dn605876.aspx
        public async Task<ILSConnection> ConnectAsync()
        {
            if (!await PingAsync()) throw new LsConnectionException("We cannot find the server . please check the Ip first");
            try
            {
                await Client.ConnectAsync(PlcModel.IP, PlcModel.PortNumber);
                Connected = true;
                NetworkStream = Client.GetStream();
            }
            catch (Exception ex)
            {
                throw new LsConnectionException($"LSConnection connect error : {ex.Message}", ex);
            }
            return this;
        }

        public void Disconnect()
        {
            Connected = false;
            Client.Dispose();
            MakeEmpty();
        }

        public void MakeEmpty()
        {
            NetworkStream = null;
        }

        public void SetDefaultPlcModel(IPlcModel plcModel) => DefaultPlcModel = plcModel;

        public async Task<IPlcResponse> SendMessageAsync(IPlcRequest request)
        {
            await NetworkStream.WriteAsync(request.Data, 0, request.Data.Length);
            byte[] response = new byte[LsConnectionStatics.MaxPlcResponseLength];
            var bytesCount = await NetworkStream.ReadAsync(response, 0, response.Length);
            return new PlcResponse
            {
                RawResponse =
                response.SubArray(0, bytesCount),
                CreatedOn = DateTime.Now,
                ResponsedOn = DateTime.Now
            };
        }

        public async Task<IPlcResponse> ReadAsync<T>(IPacket<T> packet) => await SendMessageAsync(new PlcRequest
        {
            PacketInfo = (PacketInfo)packet,
            CreatedOn = DateTime.Now,
            StartedOn = DateTime.Now,
            RequestedFrom = "",
            Data = packet.RequestPacketHeader
        });

        public async Task<IPlcResponse> WriteAsync<T>(IPacket<T> packet) => await SendMessageAsync(new PlcRequest
        {
            PacketInfo = (PacketInfo)packet,
            CreatedOn = DateTime.Now,
            StartedOn = DateTime.Now,
            RequestedFrom = "",
            Data = packet.RequestPacketHeader
        });

        public async Task<IPlcResponse> ReadAsync<T>(ILsAddress address) =>
            await ReadAsync<T>(new Packet<T>(address));

        public async Task<IPlcResponse> WriteAsync<T>(ILsAddress address, T value) =>
            await WriteAsync(new Packet<T>(value, address));

        public async Task<bool> PingAsync()
        {
            Ping ping = new Ping();
            try
            {
                if (PlcModel.IP == null) throw new LsPingException("Ip is not seted yet");
                PingReply reply = await ping.SendPingAsync(PlcModel.IP, /*time out*/ 500,
                    Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), new PingOptions(64, true));
                return reply != null && reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                Disconnect();
                return false;
            }
            catch (Exception e) { throw new LsPingException(e.Message, e); }
        }
    }
}