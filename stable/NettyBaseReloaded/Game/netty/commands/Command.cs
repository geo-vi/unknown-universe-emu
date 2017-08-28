using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.packet;

namespace NettyBaseReloaded.Game.netty.commands
{
    class Command
    { 
        public byte[] Bytes { get; }

        public bool IsNewClient { get; }

        public Command(byte[] bytes, bool isNewClient)
        {
            Bytes = bytes;
            IsNewClient = isNewClient;
        }
    }
}
