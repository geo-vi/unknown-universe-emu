using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.new_client.requests
{
    public class AttackAbortLaserRequest
    {
        public const short ID = 4592;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            parser.readShort();
        }
    }
}