using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using NettyStatusBot.network;

namespace NettyStatusBot.modules
{
    class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingServer()
        {
            //todo;
        }
    }
}
