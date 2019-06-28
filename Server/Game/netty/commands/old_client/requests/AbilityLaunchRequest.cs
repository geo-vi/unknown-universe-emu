using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class AbilityLaunchRequest
    {
        public const short ID = 26418;

        public int selectedAbilityId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            selectedAbilityId = parser.readInt();
        }
    }
}
