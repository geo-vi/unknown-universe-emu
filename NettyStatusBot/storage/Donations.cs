using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace NettyStatusBot.storage
{
    static class Donations
    {
        public static Dictionary<SocketUser, string> ClaimRequests = new Dictionary<SocketUser, string>();
    }
}
