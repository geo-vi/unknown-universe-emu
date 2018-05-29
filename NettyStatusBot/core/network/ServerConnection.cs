using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NettyStatusBot.core.network
{
    class ServerConnection
    {
        private TcpClient _client;

        public bool IsConnected => _client.Connected;

        public event EventHandler ReceivedPacket;

        public async Task Write(string message)
        {
            var utfBytes = Encoding.UTF8.GetBytes(message);
            using (var stream = _client.GetStream())
            {
                await stream.WriteAsync(utfBytes, 0, utfBytes.Length);
            }
        }
    }
}
