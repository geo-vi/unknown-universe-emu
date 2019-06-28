using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class QuestAcceptRequest
    {
        public const short ID = 8886;

        public int questId;
        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            questId = parser.readInt();
        }
    }
}
