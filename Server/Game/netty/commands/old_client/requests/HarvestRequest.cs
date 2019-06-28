using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class HarvestRequest
    {
        public const short ID = 27140;

        public string itemHash;

        public void readCommand(IByteBuffer bytes)
        {
            var parse = new ByteParser(bytes);
            itemHash = parse.readUTF();
        }
    }
}
