using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NettyStatusBot.core.network;
using NettyStatusBot.core.network.packets;
using NettyStatusBot.utils;

namespace NettyStatusBot.core
{
    class DiscordStatusUpdater
    {
        public static DiscordStatusUpdater _instance;

        public const ulong STATUS_CHANNEL_ID = 451001938809716748;

        private readonly DiscordSocketClient _client;

        public DiscordStatusUpdater(DiscordSocketClient client)
        {
            _client = client;
            _client.Ready += ClientOnReady;
            ModuleLookup.PacketReceived += (s, e) =>
            {
                if (e is PongPacket pong)
                {
                    Program.ServerStatus.PlayersOnline = pong.PlayersOnline;
                    Program.ServerStatus.Runtime = pong.RunningTime;
                }
            };
        }

        private async Task ClientOnReady()
        {
            _instance = this;
            _channel = _client.GetChannel(STATUS_CHANNEL_ID) as SocketTextChannel;
            await _client.SetGameAsync("Unknown Universe");
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await ServerConnection._instance.Write("ping");
            await Task.Factory.StartNew(StatusChannelUpdate);
        }

        private ServerStatus _lastStatus;

        public async Task StatusChannelUpdate()
        {
            await SendMessage();
            while (true)
            {
                if (_lastStatus.Maintenance != Program.ServerStatus.Maintenance || _lastStatus.Online != Program.ServerStatus.Online || _lastStatus.PlayersOnline != Program.ServerStatus.PlayersOnline)
                {
                    await SendMessage();
                    _lastStatus = Program.ServerStatus;
                }
                
                await Task.Delay(1000);
            }
        }

        private SocketTextChannel _channel;

        public async Task SendMessage()
        {
            await Clean();
            if (Program.ServerStatus.Maintenance)
            {
                await _channel.SendMessageAsync("", false, MessageBuilder.GetMaintenanceMsg());
            }
            else if (Program.ServerStatus.Online)
            {
                await _channel.SendMessageAsync("", false, MessageBuilder.GetOnlineMsg());
                await _channel.SendMessageAsync("", false, MessageBuilder.GetEventsRunning());
            }
            else
            {
                await _channel.SendMessageAsync("", false, MessageBuilder.GetOfflineMsg());
            }
        }

        public async Task Clean()
        {
            var messages = await _channel.GetMessagesAsync().Flatten();
            await _channel.DeleteMessagesAsync(messages);
        }
    }
}
