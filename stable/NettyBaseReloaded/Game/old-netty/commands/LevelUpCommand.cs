using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class LevelUpCommand
    {
        public const short ID = 32247;

        public static byte[] write(int uid, int newLevel)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(uid);
            cmd.Integer(newLevel);
            return cmd.ToByteArray();
        }
    }
}
