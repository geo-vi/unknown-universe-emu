using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class LabRefinementRequest
    {
        public const short ID = 6752;

        public OreCountModule toProduce;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            toProduce = new OreCountModule(null,0);
            toProduce.read(parser);
        }
    }
}
