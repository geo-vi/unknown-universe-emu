using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NettyStatusBot.Properties;

namespace NettyStatusBot.core
{
    class Updater
    {
        private DiscordSocketClient Client;

        public Updater(DiscordSocketClient client)
        {
            Client = client;
            Client.Ready += ClientOnReady;
        }

        private async Task ClientOnReady()
        {
            var actions = new BotActions(Client);
            await actions.SetGameState();
            await actions.ChangeStatus();
            //await StatusChannel();
            await Task.Factory.StartNew(RunServerLoop);
        }

        public static TcpClient TcpClient;
        private const ulong STATUS_CHANNEL_ID = 396274065603559424;

        public async Task StatusChannel()
        {
            var channel = Client.GetChannel(STATUS_CHANNEL_ID) as SocketTextChannel;
            if (channel == null)
            {
                Console.WriteLine("Error=Status Channel not found");
                return;
            }

            //await channel.SendMessageAsync("", false, GetOfflineMsg().Build());
            if (BotConfiguration.DISPLAY_MAINTENANCE_STATUS)
            {
                var messages = await channel.GetMessagesAsync().Flatten();

                await channel.DeleteMessagesAsync(messages);
                await channel.SendMessageAsync("", false, GetMaintenanceMsg());
                while (BotConfiguration.DISPLAY_MAINTENANCE_STATUS)
                    await Task.Delay(1000);
            }
            if (TcpClient != null && TcpClient.Connected) return;
            
            if (!ServerIsOnline(out TcpClient))
            {
                await channel.SendMessageAsync("", false, GetOfflineMsg());
            }
            else
            {
                await RunServerChecker(channel, TcpClient.GetStream());
            }
        }

        private async Task RunServerChecker(SocketTextChannel channel, NetworkStream stream)
        {
            try
            {
                SendRelay();
                while (!BotConfiguration.DISPLAY_MAINTENANCE_STATUS)
                {
                    byte[] msgBytes = new byte[1024];
                    int respLength =
                        stream.Read(msgBytes, 0, msgBytes.Length); //gets next 128 bytes when sent to client
                    var messageReceived = (System.Text.Encoding.ASCII.GetString(msgBytes));
                    var split = messageReceived.Split('|');
                    var messages = await channel.GetMessagesAsync().Flatten();
                    await channel.DeleteMessagesAsync(messages);
                    await channel.SendMessageAsync("", false,
                        GetOnlineMsg(int.Parse(split[1]), 0));
                    await channel.SendMessageAsync("", false, GetEventsRunning());
                    await stream.FlushAsync();
                }
            }
            catch (Exception)
            {
                var messages = await channel.GetMessagesAsync().Flatten();
                await channel.DeleteMessagesAsync(messages);
            }
        }

        public async void RunServerLoop()
        {
            while (true)
            {
                await StatusChannel();
            }
        }

        public bool ServerIsOnline(out TcpClient client)
        {
            try
            {
                client = new TcpClient("server1.univ3rse.com", 7778);
                return true;
            }
            catch (Exception)
            {
                client = null;
                return false;
            }
        }

        #region Messages
        private EmbedBuilder GetOfflineMsg()
        {
            var builder = new EmbedBuilder();
            builder.Title = "Current server status";
            builder.Description = "Server is OFFLINE";
            builder.Color = Color.Red;
            builder.AddField("Please PM the @La Familia", "Expect us back on soon, running!", true);
            return builder;
        }

        private EmbedBuilder GetOnlineMsg(int players, int hours)
        {
            var builder = new EmbedBuilder();
            builder.Title = "Current server status";
            builder.Description = "Server is ONLINE (runtime to be fixed asap).";
            builder.Color = Color.Green;
            builder.AddField("Currently online players", players.ToString(), true);
            return builder;
        }

        private EmbedBuilder GetEventsRunning()
        {
            var builder = new EmbedBuilder();
            builder.Title = "Events running:";
            builder.Description = "No events running.";
            builder.Color = Color.DarkGrey;
            return builder;
        }

        private EmbedBuilder GetMaintenanceMsg()
        {
            var builder = new EmbedBuilder();
            builder.AddField("Currently server is undergoing maintenance.",
                "Expect restarts, perhaps a bit unstable to play now", true);
            builder.AddField("Unknown Universe team suggests",
                "not to play now, rather wait a while until maintenance is over.");
            builder.Color = Color.LightOrange;
            return builder;
        }
        #endregion

        public static void SendRelay()
        {
            try
            {
                var message = System.Text.Encoding.ASCII.GetBytes("ping");
                var stream = TcpClient.GetStream();
                stream.Write(message, 0, message.Length);
            }
            catch (Exception)
            {

            }
        }
    }
}
