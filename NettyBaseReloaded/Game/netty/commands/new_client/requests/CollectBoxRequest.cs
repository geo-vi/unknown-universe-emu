using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    class CollectBoxRequest
    {
        public const short ID = 28222;

        public string itemHash { get; set; }
        public int var2 { get; set; }
        public int var3 { get; set; }
        public int var4 { get; set; }
        public int var5 { get; set; }

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            var2 = parser.readInt();
            var2 = var2 << 2 | var2 >> 30;
            parser.readShort();
            itemHash = parser.readUTF();
            var4 = parser.readInt();
            var4 = var4 >> 14 | var4 << 18;
            parser.readShort();
            var3 = parser.readInt();
            var3 = var3 << 16 | var3 >> 16;
            var5 = parser.readInt();
            var5 = var5 << 6 | var5 >> 26;
        }
    }
}
