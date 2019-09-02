using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class ShipWarpWindowCommand
    {
        public const short ID = 32348;

        public static Command write(int jumpVoucherCount, int uridium, bool isNearSpacestation, List<ShipWarpModule> ships)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(jumpVoucherCount);
            cmd.Integer(uridium);
            cmd.Boolean(isNearSpacestation);
            cmd.Integer(ships.Count);
            foreach (var loc1 in ships)
            {
                cmd.AddBytes(loc1.write());
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}