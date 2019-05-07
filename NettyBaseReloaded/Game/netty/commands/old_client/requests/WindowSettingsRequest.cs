using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class WindowSettingsRequest
    {
        public const short ID = 19710;

        public int clientResolutionId = 0;
      
        public string windowSettings = "";
      
        public string resizableWindows = "";
      
        public int minimapScale = 0;
      
        public string mainmenuPosition = "";
      
        public string slotmenuPosition = "";
      
        public string slotMenuOrder = "";
      
        public string slotmenuPremiumPosition = "";
      
        public string slotMenuPremiumOrder = "";
      
        public string barStatus = "";

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            this.clientResolutionId = parser.readShort();
            this.windowSettings = parser.readUTF();
            this.resizableWindows = parser.readUTF();
            this.minimapScale = parser.readInt();
            this.mainmenuPosition = parser.readUTF();
            this.slotmenuPosition = parser.readUTF();
            this.slotMenuOrder = parser.readUTF();
            this.slotmenuPremiumPosition = parser.readUTF();
            this.slotMenuPremiumOrder = parser.readUTF();
            this.barStatus = parser.readUTF();
        }
    }
}
