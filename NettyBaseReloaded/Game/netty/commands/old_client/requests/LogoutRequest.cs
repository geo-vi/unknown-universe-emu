using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class LogoutRequest
    {
        public const short ID = 7987;

        public short request;

        public void readCommand(byte[] bytes)
        {
            var p = new ByteParser(bytes);
            request = p.readShort();
        }
    }
}
