using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using System;

namespace Server.Game.objects.maps.objects
{
    class Box : Collectable
    {
        public Box(int id, string hash, Vector position, Spacemap map) : base(id, hash, position, map)
        {
        }
    }
}
