using Server.Game.objects.maps;
using System;

namespace Server.Game.objects.implementable
{
    abstract class Collectable : GameObject
    {
        public string Hash { get; set; }
        protected Collectable(int id, string hash, Vector position, Spacemap map) : base(id, position, map, 2000)
        {
            Hash = hash;
        }
    }
}
