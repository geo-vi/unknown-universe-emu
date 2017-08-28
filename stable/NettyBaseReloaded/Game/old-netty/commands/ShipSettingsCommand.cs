using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipSettingsCommand
    {
        public const short ID = 12067;

        public static byte[] write(string quickbarSlots, string quickbarSlotsPremium, int selectedLaser,
            int selectedRocket, int selectedHellstormRocket)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(quickbarSlots);
            cmd.UTF(quickbarSlotsPremium);
            cmd.Integer(selectedLaser);
            cmd.Integer(selectedRocket);
            cmd.Integer(selectedHellstormRocket);
            return cmd.ToByteArray();
        }
    }
}
