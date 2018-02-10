using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;

namespace NettyBaseReloaded.Game.objects.world.map.mines
{
    class ACM01 : Mine
    {
        public ACM01(int id, string hash, Vector pos, Spacemap map) : base(id, hash, pos, map)
        {
        }

        public override void execute(Character character)
        {
            if (!(character is Player)) return;
            Damage.Area(character.Spacemap, Position, 100, 20, DamageType.PERCENTAGE);
        }
    }
}
