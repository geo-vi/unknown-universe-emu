using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class LabRefinementRequest
    {
        public const short ID = 6752;

        public OreCountModule toProduce;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            toProduce = new OreCountModule(null, 0);
            toProduce.read(parser);
        }
    }
}
