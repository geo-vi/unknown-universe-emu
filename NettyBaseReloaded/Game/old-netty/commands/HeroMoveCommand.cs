using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class HeroMoveCommand
    {
        public const short ID = 24000;

        public static byte[] write(int x, int y)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(x);
            cmd.Integer(y);
            return cmd.ToByteArray();
        }
    }
}
