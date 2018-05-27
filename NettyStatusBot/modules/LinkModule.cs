using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace NettyStatusBot.modules
{
    [Group("link")]
    class LinkModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task Base()
        {
            await ReplyAsync("```Steps to linking your Game account with Discord" +
                             "\n - Get your session id from view-source:http://ge1.beta.univ3rse.com/internalMapRevolution" +
                             "\n - Example of how session id field looks:" +
                             "\"sessionID\": \"7e82tmo8s257ebf4l7vof4r060\"" +
                             "\nlink account [sessionid]" +
                             "```");
        }

        [Command("account")]
        public async Task Account(string sessionId)
        {
            await ReplyAsync("Successfully linked Discord account.");
        }
    }
}
