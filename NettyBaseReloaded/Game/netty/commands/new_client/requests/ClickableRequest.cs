using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    class ClickableRequest
    {
        public const short ID = 1898;

        public int clickableId;

        public void readCommand(IByteBuffer bytes)
        {
            var p = new ByteParser(bytes);
            clickableId = p.readInt();
            clickableId = clickableId << 10 | clickableId >> 22;
        }
    }
}
