using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class ClientResolutionChangeRequest
    {
        public const short ID = 23816;

        public int resolutionId;
        public int width;
        public int height;

        public void readCommand(IByteBuffer bytes)
        {
            var cmd = new ByteParser(bytes);
            cmd.readShort();
            resolutionId = cmd.readInt();
            width = cmd.readInt();
            height = cmd.readInt();
        }
    }
}
