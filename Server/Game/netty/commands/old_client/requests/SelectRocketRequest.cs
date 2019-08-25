using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class SelectRocketRequest
    {
        public const short ID = 15849;

        public AmmunitionTypeModule rocketType;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            rocketType = new AmmunitionTypeModule(parser.readShort());
        }
    }
}
