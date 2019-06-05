using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
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
