using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class HellstormStatusCommand
    {
        public const short ID = 24380;

        public static Command write(List<int> equippedLauncherTypes, AmmunitionTypeModule rocketType, int currentLoad)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(equippedLauncherTypes.Count);
            foreach (var type in equippedLauncherTypes)
            {
                cmd.Integer(type);
            }
            cmd.AddBytes(rocketType.write());
            cmd.Integer(currentLoad);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}