using Server.Game.controllers.implementable;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class ExplosivesController : ServerImplementedController
    {
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Explosives Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
        }
    }
}
