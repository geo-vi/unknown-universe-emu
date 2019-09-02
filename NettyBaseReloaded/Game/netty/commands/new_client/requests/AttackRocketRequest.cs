using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    public class AttackRocketRequest
    {
        public const short ID = 22336;

        public int targetId;
        public int x;
        public int y;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            targetId = parser.readInt();
            targetId = (int)(((uint)targetId << 4) | ((uint)targetId >> 28));
            y = parser.readInt();
            y = (int)(((uint)x >> 12) | ((uint)x << 20));
            x = parser.readInt();
            x = (int)(((uint)y >> 16) | ((uint)y << 16));
        }
    }
}