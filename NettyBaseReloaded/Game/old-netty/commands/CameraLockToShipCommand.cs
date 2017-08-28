using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class CameraLockToShipCommand
    {
        public const short ID = 24759;

        public static byte[] write(int lockedShipUserID, float zoomFactor, float duration)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(lockedShipUserID);
            cmd.Float(zoomFactor);
            cmd.Float(duration);
            return cmd.ToByteArray();
        }
    }
}
