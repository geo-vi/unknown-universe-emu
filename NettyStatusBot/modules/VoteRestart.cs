using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace NettyStatusBot.Modules
{
    class VoteRestart : ModuleBase<SocketCommandContext>
    {
        public static List<SocketUser> VotedForRestart = new List<SocketUser>();

        [Command("vote restart")]
        public async Task Vote()
        {
            var author = Context.Message.Author;
            if (VotedForRestart.Contains(author))
            {
                await ReplyAsync("Can't vote twice");
            }
            else
            {
                await ReplyAsync("Voted for restart");
                VotedForRestart.Add(author);
            }
            await ReplyAsync(VotedForRestart.Count + " votes for restarting the server.");
        }
    }
}
