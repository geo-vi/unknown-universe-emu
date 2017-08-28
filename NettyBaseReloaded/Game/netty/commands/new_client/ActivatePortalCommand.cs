using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class ActivatePortalCommand
    {
        public const short ID = 8162;

        public static Command write(int mapID, int portalId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(mapID >> 8 | mapID << 24);
            cmd.Integer(portalId << 13 | portalId >> 19);
            cmd.Short(-32636);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
