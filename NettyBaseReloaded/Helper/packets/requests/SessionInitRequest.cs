using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.packets.requests
{
    class SessionInitRequest : IRequest
    {
        public const string Prefix = "sini";

        public string DiscordId;

        public string DiscordName;

        public void Read(string[] args)
        {
            DiscordId = args[1];
            DiscordName = args[2];
        }
    }
}
