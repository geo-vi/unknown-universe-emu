using Server.Game.controllers.implementable;
using Server.Game.objects;

namespace Server.Game.controllers.server
{
    /// <summary>
    /// This controller will be used for controlling map objects and their behavior.
    /// Instances in which this controller shall be used:
    /// - Map Object Added
    /// - Map Object Removed
    /// - Map Object Moved
    /// - Temporary Objects with Disposal
    /// </summary>
    class MapController : ServerImplementedController
    {
        private Spacemap ParentMap;

        public MapController(Spacemap parentMap)
        {
            ParentMap = parentMap;
        }

        public override void OnFinishInitiation()
        {
            // Create NPCs
            // Create Objects
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}
