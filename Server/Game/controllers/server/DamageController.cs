using Server.Game.controllers.implementable;
using Server.Main;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class DamageController : ServerImplementedController
    {
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Damage Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
            
        }
    }
}
