using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using NettyStatusBot.core;
using NettyStatusBot.Properties;
using NettyStatusBot.storage;

namespace NettyStatusBot.modules
{
    class Refresh : ModuleBase<SocketCommandContext>
    {
        [Command("refresh")]
        public async Task PerformRefresh()
        {
            if (BotData.PermittedUsersToControl.Contains(Context.User.Id))
            {
                new Updater(Context.Client).StatusChannel();
            }
            else await ReplyAsync("who tf are you to tell me what to do. lmao");
        }
    }
}
