using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.player.structs
{
    struct PlayerBeacon
    {
        public bool Repairing;
        public bool InPortalArea;
        public bool InTradeArea;
        public bool InRadiationArea;
        public bool InDemiZone;
        public bool InEquipmentArea;

        public void Reset()
        {
            Repairing = false;
            InPortalArea = false;
            InTradeArea = false;
            InRadiationArea = false;
            InDemiZone = false;
            InEquipmentArea = false;
        }
    }
}
