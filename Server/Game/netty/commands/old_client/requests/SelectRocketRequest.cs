using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class SelectRocketRequest
    {
        public const short ID = 15849;

        public short type = 0;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            type = parser.readShort();
        }
    }
}
