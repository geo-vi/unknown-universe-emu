using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class LegacyModule
    {
        public const short ID = 29052;

        public string message;
        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            message = parser.readUTF();
        }

        public static Command write(string message)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(message);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
