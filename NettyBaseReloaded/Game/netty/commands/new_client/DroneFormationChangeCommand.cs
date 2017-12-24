using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class DroneFormationChangeCommand
    {
        public const short ID = 32691;

        public static Command write(int userId, int selectedFormationId)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(-10056);
            cmd.Integer(selectedFormationId << 12 | selectedFormationId >> 20);
            cmd.Integer(userId << 13 | userId >> 19);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}