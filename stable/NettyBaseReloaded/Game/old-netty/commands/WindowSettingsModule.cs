using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class WindowSettingsModule
    {
        public const short ID = 12419;

        public Boolean notSet = false;
      
        public int clientResolutionId = 0;
      
        public String windowSettings = "";
      
        public String resizableWindows = "";
      
        public int minmapScale = 0;
      
        public String mainmenuPosition = "";
      
        public String barStatus = "";
      
        public String slotmenuPosition = "";
      
        public String slotMenuOrder = "";
      
        public String slotmenuPremiumPosition = "";
      
        public String slotMenuPremiumOrder = "";

        public WindowSettingsModule(bool param1 = false, int param2 = 0, string param3 = "", string param4 = "", int param5 = 0, string param6 = "", string param7 = "", string param8 = "", string param9 = "", string param10 = "", string param11 = "")
        {
            this.notSet = param1;
            this.clientResolutionId = param2;
            this.windowSettings = param3;
            this.resizableWindows = param4;
            this.minmapScale = param5;
            this.mainmenuPosition = param6;
            this.barStatus = param7;
            this.slotmenuPosition = param8;
            this.slotMenuOrder = param9;
            this.slotmenuPremiumPosition = param10;
            this.slotMenuPremiumOrder = param11;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(this.notSet);
            cmd.Integer(this.clientResolutionId);
            cmd.UTF(this.windowSettings);
            cmd.UTF(this.resizableWindows);
            cmd.Integer(this.minmapScale);
            cmd.UTF(this.mainmenuPosition);
            cmd.UTF(this.barStatus);
            cmd.UTF(this.slotmenuPosition);
            cmd.UTF(this.slotMenuOrder);
            cmd.UTF(this.slotmenuPremiumPosition);
            cmd.UTF(this.slotMenuPremiumOrder);
            return cmd.Message.ToArray();
        }
    }
}
