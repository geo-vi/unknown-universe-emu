using Server.Game.objects.entities;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps.objects
{
    class Mine : GameObject
    {
        public Mine(int id, Vector pos, Spacemap map, int range = 1000) : base(id, pos, map, range)
        {
        }
    }
}
