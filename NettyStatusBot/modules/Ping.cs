using System.Threading.Tasks;
using Discord.Commands;

namespace NettyStatusBot.Modules
{
    class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingServer()
        {
            //todo;
        }
    }
}
