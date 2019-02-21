using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NettyStatusBot.core;
using NettyStatusBot.network.packets;

namespace NettyStatusBot.network
{
    class ServerConnection
    {
        public static ServerConnection _instance = null;

        private TcpClient _client;

        private DiscordSocketClient _discordClient;

        public bool IsConnected
        {
            get
            {
                if (_client != null)
                    return _client.Connected;
                return false;
            }
        }

        public ServerConnection(DiscordSocketClient client)
        {
            _instance = this;
            _discordClient = client;
            Task.Factory.StartNew(Read);
        }

        private async Task Read()
        {
            while (true)
            {
                if (!IsConnected)
                {
                    await _discordClient.SetStatusAsync(UserStatus.DoNotDisturb);
                    TryConnection();
                }
                else
                {
                    await _discordClient.SetStatusAsync(UserStatus.Online);
                    await ReadPacket();
                }
            }
        }

        private async void TryConnection()
        {
            try
            {
                _client = new TcpClient { NoDelay = true, ReceiveBufferSize = 1024 };
                _client.Connect("127.0.0.1", 7778);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            if (IsConnected)
                await Write("sini|" + _discordClient.CurrentUser.Id + "|" + _discordClient.CurrentUser.Username + "#"+ _discordClient.CurrentUser.DiscriminatorValue);
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
