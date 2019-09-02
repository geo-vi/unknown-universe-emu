using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class EquipModuleRequest
    {
        public const short ID = 25418;

        public int battleStationId;
        public int itemId;
        public int slotId;
        public bool replace;

        public void readCommand(IByteBuffer bytes)
        {
            var cmd = new ByteParser(bytes);
            battleStationId = cmd.readInt();
            itemId = cmd.readInt();
            slotId = cmd.readInt();
            replace = cmd.readBool();
        }
    }
}
