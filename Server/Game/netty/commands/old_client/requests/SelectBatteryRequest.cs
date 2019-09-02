using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class SelectBatteryRequest
    {
        public const short ID = 10575;

        public AmmunitionTypeModule batteryType;
        
        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            batteryType = new AmmunitionTypeModule(parser.readShort());
        }
    }
}
