using Server.Game.objects;

namespace Server.Game.controllers.maps
{
    abstract class MapSubController
    {
        protected Spacemap Spacemap;
        
        protected MapSubController(Spacemap map)
        {
            Spacemap = map;
        }
    }
}