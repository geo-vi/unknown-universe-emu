using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class PetDeactivationCommand
    {
        public const short ID = 2419;

        public static Command write(int petId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
