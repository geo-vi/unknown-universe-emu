using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace NettyStatusBot.modules
{
    [Group("help")]
    class HelpModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task Base()
        {
            await ReplyAsync("```xml" +
                             "\n<Donations related = donate>" +
                             "\n<Invitation code = invitecode>" +
                             "\n<Link account = link>" +
                             "\nMore to come soon!" +
                             "```");
        }
    }
}
