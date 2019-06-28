using Server.Game.objects;
using Server.Game.objects.maps;

namespace Server.Game.controllers.maps
{
    abstract class GameObjectController : MapSubController
    {
        public GameObject GameObject;
        
        protected GameObjectController(Spacemap map, GameObject gameObject) : base(map)
        {
            GameObject = gameObject;
        }
    }
}