using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class ShipWarpRequest
    {
        public const short ID = 1450;

        public int shipType;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            shipType = parser.readInt();
        }
    }
}
