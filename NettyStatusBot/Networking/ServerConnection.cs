using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NettyStatusBot.Networking.packets;

namespace NettyStatusBot.Networking
{
    class ServerConnection
    {
        public static ServerConnection _instance;
        
        /// <summary>
        /// sini|" + _discordClient.CurrentUser.Id + "|" + _discordClient.CurrentUser.Username + "#"+ _discordClient.CurrentUser.DiscriminatorValue
        /// </summary>
        /// <param name="port"></param>

        private DiscordSocketClient _client;

        private XSocket _socket;

        private int _port;

        private bool _connectionFactor;

        /// <summary>
        /// Constructor -> Needs the Discord client for connection successful packet
        /// </summary>
        /// <param name="client"></param>
        /// <param name="port"></param>
        public ServerConnection(DiscordSocketClient client, int port)
        {
            _instance = this;
            _client = client;
            _port = port;
            Task.Factory.StartNew(ConnectionMonitor);
        }

        private void SocketOnConnectionClosedEvent(object sender, EventArgs e)
        {
            _connectionFactor = false;
            Task.Factory.StartNew(ConnectionMonitor);
        }

        /// <summary>
        /// Monitors the connections if xSocket is connected else will attempt @TryConnection
        /// </summary>
        private async Task ConnectionMonitor()
        {
            while (!_connectionFactor)
            {
                await TryConnection();
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Attempting a connection
        /// First will connect using TcpClient => Preventing bugs then transferring it to XSocket
        /// </summary>
        private async Task TryConnection()
        {
            try
            {
                var tcpClient = new TcpClient();
                await tcpClient.ConnectAsync("127.0.0.1", _port);
                if (tcpClient.Connected)
                {
                    _socket = new XSocket(tcpClient.Client);
                    _socket.OnReceive += XSocketOnOnReceive;
                    _socket.ConnectionClosedEvent += SocketOnConnectionClosedEvent;
                    _socket.Read(true);
                    _socket.Write("sini|" + _client.CurrentUser.Id + "|" + _client.CurrentUser.Username + "#" +
                                  _client.CurrentUser.DiscriminatorValue);
                    _connectionFactor = true;
                }
            }
            catch (Exception)
            {
                _connectionFactor = false;
            }
        }

        private void XSocketOnOnReceive(object sender, EventArgs e)
        {
            var args = (StringArgs) e;
            HandlerFinder.Handle(_client, args.Packet);
        }

        public void Send(string packet)
        {
            try
            {
                if (_connectionFactor)
                    _socket.Write(packet);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed sending packet.");
            }
        }
    }
}
