using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    public class AttackRocketRequest
    {
        public const short ID = 16203;

        public int targetId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            targetId = parser.readInt();
        }
    }
}