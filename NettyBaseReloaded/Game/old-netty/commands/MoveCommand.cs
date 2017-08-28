using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class MoveCommand
    {
        public const short ID = 20502;

        public static byte[] write(int userId, int x,int y, int timeToTarget)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Integer(timeToTarget);
            return cmd.ToByteArray();
        }
    }
}
