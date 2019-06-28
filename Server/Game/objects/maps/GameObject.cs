using Server.Game.objects.entities;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps
{
    abstract class GameObject
    {
        public int Id { get; }
        public Vector Position { get; set; }
        public Spacemap Spacemap { get; set; }
        public int Range { get; set; }

        public int VirtualWorldId { get; set; }

        protected GameObject(int id, Vector pos, Spacemap map, int range = 1000)
        {
            Id = id;
            Position = pos;
            Spacemap = map;
            Range = range;
        }
    }
}
