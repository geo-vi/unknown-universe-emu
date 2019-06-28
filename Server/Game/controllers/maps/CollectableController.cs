using Server.Game.objects;
using Server.Game.objects.maps;

namespace Server.Game.controllers.maps
{
    class CollectableController : GameObjectController
    {
        public CollectableController(Spacemap map, GameObject gameObject) : base(map, gameObject)
        {
        }
    }
}