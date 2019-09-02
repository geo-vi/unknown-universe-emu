using Server.Game.controllers.implementable;
using Server.Main;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class EffectsController : ServerImplementedController
    {
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Effects Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
        }
    }
}