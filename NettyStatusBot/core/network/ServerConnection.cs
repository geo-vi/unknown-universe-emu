using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NettyStatusBot.core.network.packets;

namespace NettyStatusBot.core.network
{
    class ServerConnection
    {
        public static ServerConnection _instance = null;

        private TcpClient _client;

        public bool IsConnected
        {
            get
            {
                if (_client != null)
                    return _client.Connected;
                return false;
            }
        }

        public ServerConnection()
        {
            _instance = this;
            Task.Factory.StartNew(Read);
        }

        private async Task Read()
        {
            while (true)
            {
                if (!IsConnected)
                {
                    TryConnection();
                }
                else
                {
                    await ReadPacket();
                }

                Program.ServerStatus.Online = IsConnected;
            }
        }

        private void TryConnection()
        {
            try
            {
                _client = new TcpClient("server1.univ3rse.com", 7778) { NoDelay = true, ReceiveBufferSize = 1024 };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task ReadPacket()
        {
            try
            {
                var stream = _client.GetStream();

                byte[] msgBytes = new byte[1024];
                int respLength =
                    await stream.ReadAsync(msgBytes, 0, msgBytes.Length); //gets next 128 bytes when sent to client
                var messageReceived = System.Text.Encoding.UTF8.GetString(msgBytes);
                ModuleLookup.Find(messageReceived);
                await stream.FlushAsync();
            }
            catch (Exception)
            {
            }
        }

        public async Task Write(string message)
        {
            var utfBytes = Encoding.UTF8.GetBytes(message);
            try
            {
                var stream = _client.GetStream();

                await stream.WriteAsync(utfBytes, 0, utfBytes.Length);
            }
            catch (Exception)
            {

            }
        }
    }
}
