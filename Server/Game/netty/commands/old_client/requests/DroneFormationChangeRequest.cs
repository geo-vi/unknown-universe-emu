using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class DroneFormationChangeRequest
    {
        public const short ID = 22456;

        public int selectedFormationId;

        public void readCommand(IByteBuffer bytes)
        {
            var cmd = new ByteParser(bytes);
            selectedFormationId = cmd.readInt();
        }
    }
}
