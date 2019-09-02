using Server.Game.controllers.implementable;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class HealingController : ServerImplementedController
    {
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Healing Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
        }
    }
}
