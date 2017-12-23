using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class LevelUpCommand
    {
        public const short ID = 32247;

        public static Command write(int userId, int newLevel)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.Integer(newLevel);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
