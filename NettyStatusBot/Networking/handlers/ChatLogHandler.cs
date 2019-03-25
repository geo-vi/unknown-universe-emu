using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using NettyStatusBot.Networking.packets;

namespace NettyStatusBot.Networking.handlers
{
    class ChatLogHandler : IHandler
    {
        public void execute(DiscordSocketClient client, string packet)
        {
            var chatLog = new ChatLogPacket();
            chatLog.readPacket(packet);

            var channel = client.GetChannel(557483130580107265) as IMessageChannel;
            channel?.SendMessageAsync($"```[{chatLog.Username} : {chatLog.ChatRank} | UID : {chatLog.Id}]" + Environment.NewLine + "" + chatLog.Message + "```");
        }
    }
}
