using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using NettyStatusBot.core.network;

namespace NettyStatusBot.modules
{
    class AdministratorModule : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        public async Task Kick()
        {
            await ReplyAsync("Possible options: \n`kick [id]\nkick [name]`");
        }

        [Command("kick")]
        public async Task Kick(int playerId)
        {
            await ReplyAsync($"Executing command.\n`Kicking ID: {playerId}`");
            await ServerConnection._instance.Write("kick|" + playerId);
        }
    }
}
