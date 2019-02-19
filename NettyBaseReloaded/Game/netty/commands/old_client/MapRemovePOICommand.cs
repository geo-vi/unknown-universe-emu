using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class MapRemovePOICommand
    {
        public const short ID = 7044;

        public static Command write(string poiId)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(poiId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
