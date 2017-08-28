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
        public const short ID = 28554;

        public static Command write(int mapID, int unknownVar)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(mapID >> 14 | mapID << 18);
            cmd.Short(15041);
            cmd.Short(-851);
            cmd.Integer(unknownVar << 12 | unknownVar >> 20);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
