using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NettyStatusBot.core;
using NettyStatusBot.network;

namespace NettyStatusBot.modules
{
    class RestartServer : ModuleBase<SocketCommandContext>
    {
        [Command("restart")]
        public async Task Restart()
        {
            if (Context.Channel.Id != 368769968759767051)
            {
                await ReplyAsync("ask someone else");
                return;
            }

            await ReplyAsync("Restarting server...");
            await ServerConnection._instance.Write("kill");
        }
    }
}
