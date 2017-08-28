using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class PetGearAddCommand
    {
        public const short ID = 29895;

        public static byte[] write(PetGearTypeModule gearType, int level, int amount, bool enabled)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(gearType.write());
            cmd.Integer(level);
            cmd.Integer(amount);
            cmd.Boolean(enabled);
            return cmd.ToByteArray();
        }
    }
}
