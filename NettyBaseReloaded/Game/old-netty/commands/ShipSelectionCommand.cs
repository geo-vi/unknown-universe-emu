using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipSelectionCommand
    {
        public const short ID = 19586;

        public static byte[] write(int userId, int shipType, int shield, int shieldMax, int hitpoints,int hitpointsMax, int nanoHull, int maxNanoHull,
            bool shieldSkill)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.Integer(shipType);
            cmd.Integer(shield);
            cmd.Integer(shieldMax);
            cmd.Integer(hitpoints);
            cmd.Integer(hitpointsMax);
            cmd.Integer(nanoHull);
            cmd.Integer(maxNanoHull);
            cmd.Boolean(shieldSkill);
            return cmd.ToByteArray();
        }
    }
}
