using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.packet;

namespace NettyBaseReloaded.Game.netty
{
    class Packet
    {
        public static packet.Builder Builder = new Builder();
        public static packet.Handler Handler = new Handler();

        public static void AddCommands()
        {
            
        }
    }
}
