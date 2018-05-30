using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class PetShieldUpdateCommand
    {
        public const short ID = 8653;

        public static Command write(int petShieldNow, int petShieldMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petShieldNow);
            cmd.Integer(petShieldMax);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
