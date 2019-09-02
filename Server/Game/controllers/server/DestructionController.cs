using Server.Game.controllers.implementable;
using Server.Main;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    /// <summary>
    /// This controller will be used for base of destructing attackables and objects
    /// Instances in which this controller will be called or used:
    /// - Player explosion / destruction
    /// - Pet explosion / destruction
    /// - Npc explosion / destruction
    /// 
    /// </summary>
    class DestructionController : ServerImplementedController
    {
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Destruction Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
        }
    }
}
