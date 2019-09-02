using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class DroneFormationChangeRequest
    {
        public const short ID = 22456;

        public int selectedFormationId;

        public void readCommand(IByteBuffer bytes)
        {
            var cmd = new ByteParser(bytes);
            selectedFormationId = cmd.readInt();
        }
    }
}
