using Server.Game.objects;
using Server.Game.objects.maps.objects;

namespace Server.Game.controllers.maps
{
    class MineController : GameObjectController
    {
        public Mine Mine { get; set; }
        
        public MineController(Spacemap map, Mine mine) : base(map, mine)
        {
        }
    }
}