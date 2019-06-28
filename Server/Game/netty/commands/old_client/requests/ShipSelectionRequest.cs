using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class ShipSelectionRequest
    {
        public const short ID = 1666;

        public int targetId { get; set; }
        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            targetId = parser.readInt();
        }
    }
}
