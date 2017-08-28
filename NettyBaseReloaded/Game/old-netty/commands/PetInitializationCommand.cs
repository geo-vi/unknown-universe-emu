using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class PetInitializationCommand
    {
        public const short ID = 20448;

        public static byte[] write(bool hasPet, bool hasFuel, bool petIsAlive)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(hasPet);
            cmd.Boolean(hasFuel);
            cmd.Boolean(petIsAlive);
            return cmd.ToByteArray();
        }
    }
}
