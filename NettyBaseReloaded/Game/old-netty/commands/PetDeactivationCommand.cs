using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class PetDeactivationCommand
    {
        public const short ID = 2419;

        public static byte[] write(int petId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petId);
            return cmd.ToByteArray();
        }
    }
}
