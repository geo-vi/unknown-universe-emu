using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class MoveCommand
    {
        public const short ID = 1771;
        public static Command write(int userId, int x, int y, int timeToTarget)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(22713);
            cmd.Short(4581);
            cmd.Integer(y >> 1 | y << 31);
            cmd.Integer(userId >> 10 | userId << 22);
            cmd.Integer(x << 6 | x >> 26);
            cmd.Integer(timeToTarget << 11 | timeToTarget >> 21);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
