using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class commandu1C
    {
        public const short ID = 23492;

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            return cmd.Message.ToArray();
        }
    }
}
