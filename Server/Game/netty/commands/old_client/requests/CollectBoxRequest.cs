using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class CollectBoxRequest
    {
        public const short ID = 29347;

        public string itemHash { get; set; }

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            itemHash = parser.readUTF();
        }
    }
}
