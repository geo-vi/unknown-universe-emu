using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipDestroyedCommand
    {
        public const short ID = 11189;
        public static byte[] write(int destroyedUserId, int explosionTypeId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(destroyedUserId);
            cmd.Integer(explosionTypeId);
            return cmd.ToByteArray();
        }
    }
}
