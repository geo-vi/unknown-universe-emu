using Server.Game.objects.entities;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps.objects.assets
{
    class Jumpgate : GameObject, IClickable
    {
        public void Click()
        {

        }

        public Jumpgate(int id, Vector pos, Spacemap map, int range = 1000) : base(id, pos, map, range)
        {
        }

        public override void execute(Character character)
        {
            throw new System.NotImplementedException();
        }
    }
}
