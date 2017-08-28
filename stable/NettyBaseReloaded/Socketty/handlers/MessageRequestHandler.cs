using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Socketty.handlers
{
    class MessageRequestHandler : IHandler
    {
        public void execute(string packet)
        {
            var split = packet.Split('|');
            //foreach (var spacemap in World.StorageManager.Spacemaps)
            //    GameClient.SendToSpacemap(spacemap.Value, Builder.LegacyModule("0|A|STD|" + split[1] + "").Bytes);
            Console.WriteLine("/global " + split[1]);
        }
    }
}
