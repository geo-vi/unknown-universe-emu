using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class PetFuelUpdateCommand
    {
        public const short ID = 22079;

        public static Command write(int petFuelNow, int petFuelMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petFuelNow);
            cmd.Integer(petFuelMax);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
