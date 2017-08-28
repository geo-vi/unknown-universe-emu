using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipRemoveCommand
    {
        public const short ID = 29006;

        public static byte[] write(int userId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            return cmd.ToByteArray();
        }
    }
}
