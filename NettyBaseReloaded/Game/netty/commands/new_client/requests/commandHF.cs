using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    class commandHF
    {
        public const short ID = 4342;

        public string originSlotbar;
        public int originSlotId;
        public string targetSlotbar;
        public int targetSlotId;
        public string itemId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            originSlotId = parser.readInt();
            originSlotId = originSlotId >> 8 | originSlotId << 24;
            originSlotbar = parser.readUTF();
            itemId = parser.readUTF();
            targetSlotId = parser.readInt();
            targetSlotId = targetSlotId >> 16 | targetSlotId << 16;
            targetSlotbar = parser.readUTF();
        }
    }
}
