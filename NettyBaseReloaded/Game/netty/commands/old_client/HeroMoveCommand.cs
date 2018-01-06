using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class HeroMoveCommand
    {
        public const short ID = 24000;
        public static Command write(int x, int y)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(x);
            cmd.Integer(y);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
