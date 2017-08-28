using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class commandK13
    {
        public const short ID = 4017;

        public static short N2K = 2;
        public static short DEFAULT = 0;
        public static short ALLY = 1;

        public short relation;

        public commandK13(short relation)
        {
            this.relation = relation;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(relation);
            return cmd.Message.ToArray();
        }

    }
}
