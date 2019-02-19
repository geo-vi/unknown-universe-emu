using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.mines
{
    class EMPM01 : Mine
    {
        public override int MineType => 2;

        public EMPM01(int id, string hash, Vector pos, Spacemap map) : base(id, hash, pos, map)
        {
        }

        public override void Effect()
        {
            var area = Spacemap.Entities.Where(x => x.Value.Position.DistanceTo(Position) <= 1000 && x.Value is Player);
            foreach (var entry in area)
            {
                entry.Value.Controller?.Effects?.Uncloak();
            }
        }
    }
}
