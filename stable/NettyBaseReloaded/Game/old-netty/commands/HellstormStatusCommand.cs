using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class HellstormStatusCommand
    {
        public const short ID = 24380;

        public static byte[] write(List<int> equippedLauncherTypes, AmmunitionTypeModule rocketType, int currentLoad)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(equippedLauncherTypes.Count);
            foreach (var loc in equippedLauncherTypes)
            {
                cmd.Integer(loc);
            }
            cmd.AddBytes(rocketType.write());
            cmd.Integer(currentLoad);
            return cmd.ToByteArray();
        }
    }
}
