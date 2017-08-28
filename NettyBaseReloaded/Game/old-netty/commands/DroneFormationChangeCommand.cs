using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class DroneFormationChangeCommand
    {
        public const short ID = 17012;

        public static byte[] write(int uid, int selectedFormationId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(uid);
            cmd.Integer(selectedFormationId);
            return cmd.ToByteArray();
        }
    }
}
