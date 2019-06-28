using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class ShipSettingsRequest
    {
        public const short ID = 1336;

        public string quickbarSlots = "";

        public string quickbarSlotsPremium = "";

        public int selectedLaser = 0;

        public int selectedRocket = 0;

        public int selectedHellstormRocket = 0;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            quickbarSlots = parser.readUTF();
            quickbarSlotsPremium = parser.readUTF();
            selectedLaser = parser.readInt();
            selectedRocket = parser.readInt();
            selectedHellstormRocket = parser.readInt();
        }
    }
}
