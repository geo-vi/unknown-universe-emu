using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipWarpWindowCommand
    {
        public const short ID = 32348;

        public static byte[] write(int jumpVoucherCount, int uridium, bool isNearSpacestation, List<ShipWarpModule> ships)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(jumpVoucherCount);
            cmd.Integer(uridium);
            cmd.Boolean(isNearSpacestation);
            cmd.Integer(ships.Count);
            foreach (var _loc2_ in ships)
            {
                cmd.AddBytes(_loc2_.write());
            }
            return cmd.ToByteArray();
        }
    }
}
