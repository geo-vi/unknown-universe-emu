using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class PetGearAddCommand
    {
        public const short ID = 12032;

        public static Command write(PetGearTypeModule petGearTypeModule, int level, int amount, bool enabled)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(petGearTypeModule.write());
            cmd.Boolean(enabled);
            cmd.Integer(amount << 2 | amount >> 30);
            cmd.Short(8148);
            cmd.Integer(level >> 9 | level << 23);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
