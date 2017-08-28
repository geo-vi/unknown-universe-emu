using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class ShipRemoveCommand
    {
        public const short ID = 14332;

        public static Command write(int uid)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(21523);
            cmd.Integer(uid >> 9 | uid << 23);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
