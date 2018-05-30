using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using NettyStatusBot.core;
using NettyStatusBot.core.network;
using NettyStatusBot.Properties;
using NettyStatusBot.storage;

namespace NettyStatusBot.modules
{
    [Group("refresh")]
    class RefreshModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task RefreshCommand()
        {
        }

        [Command("bot")]
        public async Task RefreshBot()
        {
            if (BotData.PermittedUsersToControl.Contains(Context.User.Id))
            {
                await ServerConnection._instance.Write("ping");
            }
            else await ReplyAsync("who tf are you to tell me what to do. lmao");
        }

        [Command("rank")]
        public async Task RefreshRank()
        {
            if (BotData.PermittedUsersToControl.Contains(Context.User.Id))
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://beta.univ3rse.com/core/tasks/task.ranking.php"))
                {
                }
                await ReplyAsync("Successfully refreshed rank.");
            }
            else await ReplyAsync("Forbidden.");
        }

        [Command("server")]
        public async Task Server()
        {
            if (BotData.PermittedUsersToControl.Contains(Context.User.Id))
            {
                await ReplyAsync("Preparing server for restart...");
                await ServerConnection._instance.Write("restart");
            }
        }
    }
}
