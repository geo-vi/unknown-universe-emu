using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class LegacyModule
    {
        public const short ID = 29052;
        
        public static byte[] write(string message)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(message);
            return cmd.ToByteArray();
        }
    }
}
