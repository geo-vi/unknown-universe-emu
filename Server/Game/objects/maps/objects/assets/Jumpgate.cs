using Server.Game.objects.entities;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps.objects.assets
{
    class Jumpgate : GameObject
    {
        public Jumpgate(int id, Vector pos, Spacemap map, int range = 1000) : base(id, pos, map, range)
        {
        }
    }
}
