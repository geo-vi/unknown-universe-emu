using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class KillScreenRepairRequest
    {
        public const short ID = 3303;

        public KillScreenOptionTypeModule selection;

        public void readCommand(IByteBuffer bytes)
        {
            var p = new ByteParser(bytes);
            p.readShort();
            selection = new KillScreenOptionTypeModule(p.readShort());
        }
    }
}
