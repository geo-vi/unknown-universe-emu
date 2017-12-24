using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class DroneFormationChangeCommand
    {
        public const short ID = 17012;

        public static Command write(int userId, int selectedFormationId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.Integer(selectedFormationId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}