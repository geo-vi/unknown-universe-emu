using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class BuildStationRequest
    {
        public const short ID = 14010;

        public int battleStationId;
        public int buildTimeInMinutes;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            battleStationId = parser.readInt();
            buildTimeInMinutes = parser.readInt();
        }
    }
}
