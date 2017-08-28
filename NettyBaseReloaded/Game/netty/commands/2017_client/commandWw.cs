using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class commandWw
    {
        public const short ID = 3660;

        public static short LO = 7;

        public static short n31 = 4;

        public static short PLAIN = 0;

        public static short x1 = 2;

        public static short A36 = 3;

        public static short B4t = 1;

        public static short LOCALIZED = 5;

        public static short Yv = 6;

        public short type;

        public commandWw(short type)
        {
            this.type = type;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(type);
            return cmd.Message.ToArray();
        }

    }
}
