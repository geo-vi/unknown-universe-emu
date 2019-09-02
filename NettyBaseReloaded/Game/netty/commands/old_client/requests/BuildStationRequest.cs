using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class BuildStationRequest
    {
        public const short ID = 14010;

        public int battleStationId;
        public int buildTimeInMinutes;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            battleStationId = parser.readInt();
            buildTimeInMinutes = parser.readInt();
        }
    }
}
