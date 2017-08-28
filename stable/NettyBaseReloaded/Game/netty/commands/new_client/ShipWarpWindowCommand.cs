using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class ShipWarpWindowCommand
    {
        public const short ID = 17461;

        public static Command write(int jumpVoucherCount, int uridium, bool isNearSpacestation, List<ShipWarpModule> ships)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(isNearSpacestation);
            cmd.Integer(uridium << 9 | uridium >> 23);
            cmd.Integer(jumpVoucherCount >> 7 | jumpVoucherCount << 25);
            cmd.Short(-20299);
            cmd.Integer(ships.Count);
            foreach(var _loc2_ in ships)
            {
                cmd.AddBytes(_loc2_.write());
            }
            return new Command(cmd.ToByteArray(), true);
        }
    }
}