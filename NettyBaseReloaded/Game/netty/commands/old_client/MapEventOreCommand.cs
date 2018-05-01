using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class MapEventOreCommand
    {
        public const short ID = 15901;

        public static Command write(short eventType, OreTypeModule oreType, string hash)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(eventType);
            cmd.AddBytes(oreType.write());
            cmd.UTF(hash);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
