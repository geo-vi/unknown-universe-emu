using System.Threading.Tasks;
using Discord.Commands;
using NettyStatusBot.Networking;

namespace NettyStatusBot.Modules
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
            ServerConnection._instance.Send("kill");
        }
    }
}
