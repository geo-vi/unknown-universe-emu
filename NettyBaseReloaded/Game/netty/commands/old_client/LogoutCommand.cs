using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class LogoutCommand
    {
        public const short ID = 4743;

        public static Command write(short command)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(command);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
