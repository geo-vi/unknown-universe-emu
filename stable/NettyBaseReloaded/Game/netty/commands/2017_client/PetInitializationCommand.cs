using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class PetInitializationCommand
    {
        public const short ID = 13815;

        public static Command write(bool hasPet, bool hasFuel, bool petIsAlive)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(hasFuel);
            cmd.Boolean(petIsAlive);
            cmd.Boolean(hasPet);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
