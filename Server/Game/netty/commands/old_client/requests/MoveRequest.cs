using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class MoveRequest
    {
        public const short ID = 6417;

        public int positionX;
        public int targetY;
        public int targetX;
        public int positionY;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            this.positionX = parser.readInt();
            this.targetY = parser.readInt();
            this.targetX = parser.readInt();
            this.positionY = parser.readInt();

        }
    }
}
