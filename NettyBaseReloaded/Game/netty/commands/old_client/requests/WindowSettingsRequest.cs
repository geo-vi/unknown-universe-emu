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
            Debug.WriteLine("ClientRes = " + clientResolutionId);
            this.windowSettings = parser.readUTF();
            Debug.WriteLine(windowSettings);
            this.resizableWindows = parser.readUTF();
            Debug.WriteLine(resizableWindows);
            this.minimapScale = parser.readInt();
            Debug.WriteLine(minimapScale);
            this.mainmenuPosition = parser.readUTF();
            Debug.WriteLine(mainmenuPosition);
            this.slotmenuPosition = parser.readUTF();
            Debug.WriteLine(slotmenuPosition);
            this.slotMenuOrder = parser.readUTF();
            Debug.WriteLine(slotMenuOrder);
            this.slotmenuPremiumPosition = parser.readUTF();
            Debug.WriteLine(slotmenuPremiumPosition);
            this.slotMenuPremiumOrder = parser.readUTF();
            Debug.WriteLine(slotMenuPremiumOrder);
            this.barStatus = parser.readUTF();
            Debug.WriteLine(barStatus);
        }
    }
}
