using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class LogoutRequest
    {
        public const short ID = 7987;

        public const short REQUEST_LOGOUT = 0;  
        public const short ABORT_LOGOUT = 1;

        public short request;

        public void readCommand(IByteBuffer bytes)
        {
            var p = new ByteParser(bytes);
            request = p.readShort();
        }
    }
}
