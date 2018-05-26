using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace NettyStatusBot.core
{
    class BotActions
    {
        private DiscordSocketClient Client;

        public BotActions(DiscordSocketClient client)
        {
            Client = client;
        }

        public async Task SetGameState()
        {
            await Client.SetGameAsync("Unknown Universe", "http://beta.univ3rse.com");
        }

        public async Task ChangeStatus()
        {
            await Client.SetStatusAsync(UserStatus.DoNotDisturb);
        }
    }
}
