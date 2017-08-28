using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class LegacyModule
    {
        public const short ID = 32601;

        public static Command write(string message)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(message);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
