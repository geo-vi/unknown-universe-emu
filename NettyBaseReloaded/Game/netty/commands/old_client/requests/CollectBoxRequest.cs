using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class CollectBoxRequest
    {
        public const short ID = 29347;

        public string itemHash { get; set; }

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            itemHash = parser.readUTF();
        }
    }
}
