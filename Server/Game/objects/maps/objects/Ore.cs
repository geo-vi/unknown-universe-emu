using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using System;

namespace Server.Game.objects.maps.objects
{
    class Ore : Collectable
    {

        public override void Collect()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void execute(Character character)
        {
            throw new NotImplementedException();
        }

        public Ore(int id, string hash, Vector position, Spacemap map) : base(id, hash, position, map)
        {
        }
    }
}
