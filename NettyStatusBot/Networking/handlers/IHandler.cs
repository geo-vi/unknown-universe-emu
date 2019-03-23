using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace NettyStatusBot.Networking.handlers
{
    interface IHandler
    {
        void execute(DiscordSocketClient client, string packet);
    }
}
