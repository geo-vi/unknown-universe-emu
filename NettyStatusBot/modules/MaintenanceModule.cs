using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using NettyStatusBot.core;
using NettyStatusBot.Properties;
using NettyStatusBot.storage;

namespace NettyStatusBot.modules
{
    class MaintenanceModule : ModuleBase<SocketCommandContext>
    {
        [Command("maintenance")]
        public async Task ToggleMaintenanceMode()
        {
            if (BotData.PermittedUsersToControl.Contains(Context.User.Id))
            {
                await ReplyAsync("Understood, proceeding.");
                if (Program.ServerStatus.Maintenance)
                {
                    await ReplyAsync("Maintenance is active.");
                    Program.ServerStatus.Maintenance = false;
                }
                else
                {
                    await ReplyAsync("Maintenance is inactive.");
                    Program.ServerStatus.Maintenance = true;
                }
            }
            else await ReplyAsync("who tf are you to tell me what to do. lmao");
        }
    }
}
