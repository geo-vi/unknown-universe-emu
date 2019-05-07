using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class ShipWarpRequest
    {
        public const short ID = 1450;

        public int shipType;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            shipType = parser.readInt();
        }
    }
}
