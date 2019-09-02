using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class TradeSellOreRequest
    {
        public const short ID = 26473;
        public OreCountModule toSell;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            toSell = new OreCountModule(null, 0);
            toSell.read(parser);
        }
    }
}
