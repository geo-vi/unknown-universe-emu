using System;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    public class ItemSelectionRequest
    {
        public const short ID = 2287;

        public const short var92j = 0;
        public const short varR4M = 1;
        public const short SELECT = 0;
        public const short ACTIVATE = 1;

        public short y2g = 0;
        public short var54i = 0;
        public string itemId = "";

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            y2g = parser.readShort();
            var54i = parser.readShort();
            itemId = parser.readUTF();
        }
    }
}