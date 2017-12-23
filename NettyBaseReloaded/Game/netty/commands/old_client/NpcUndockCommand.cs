using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class NpcUndockCommand
    {
        public const short ID = 3269;

        public static Command write(int motherShipId, int minionId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(motherShipId);
            cmd.Integer(minionId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
