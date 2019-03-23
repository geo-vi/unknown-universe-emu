using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.packets.requests
{
    class DiscordChannelMessageRequest : IRequest
    {
        public const string Prefix = "dcm";

        public string Username;

        public string Content;

        public void Read(string[] args)
        {
            Username = args[1];
            Content = args[2];
        }
    }
}
