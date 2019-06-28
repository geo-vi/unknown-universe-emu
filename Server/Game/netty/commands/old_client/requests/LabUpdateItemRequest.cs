using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class LabUpdateItemRequest
    {
        public const short ID = 5583;

        public LabItemModule itemToUpdate;
        public OreCountModule updateWith;

        public LabUpdateItemRequest()
        {
            itemToUpdate = new LabItemModule(-1);
            updateWith = new OreCountModule(new OreTypeModule(-1), -1);
        }
        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            itemToUpdate.read(parser);
            updateWith.read(parser);
        }
    }
}
