using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class QuestInfoRequest
    {
        public const short ID = 28545;

        public int questId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            questId = parser.readInt();
        }
    }
}
