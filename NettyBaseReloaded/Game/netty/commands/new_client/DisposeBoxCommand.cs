using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class DisposeBoxCommand
    {
        public const short ID = 24098;

        public static Command write(string hash, bool param2)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(-15321);
            cmd.Boolean(param2);
            cmd.UTF(hash);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
