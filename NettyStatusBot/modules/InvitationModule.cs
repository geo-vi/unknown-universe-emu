using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace NettyStatusBot.modules
{
    class InvitationModule : ModuleBase<SocketCommandContext>
    {
        [Command("invitecode")]
        public async Task GetInviteCode()
        {
            await ReplyAsync("No invite codes left.");
        }
    }
}
