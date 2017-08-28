using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class CameraLockToCoordinatesCommand
    {
        public const short ID = 15097;

        public static byte[] write(int x, int y, float duration)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Float(duration);
            return cmd.ToByteArray();
        }
    }
}
