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
                if (BotConfiguration.DISPLAY_MAINTENANCE_STATUS)
                {
                    await ReplyAsync("Maintenance is active.");
                    BotConfiguration.DISPLAY_MAINTENANCE_STATUS = false;
                }
                else
                {
                    await ReplyAsync("Maintenance is inactive.");
                    BotConfiguration.DISPLAY_MAINTENANCE_STATUS = true;
                }
                Updater.SendRelay();
            }
            else await ReplyAsync("who tf are you to tell me what to do. lmao");
        }
    }
}
