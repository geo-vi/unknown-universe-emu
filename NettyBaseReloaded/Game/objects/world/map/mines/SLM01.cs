using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.map.mines
{
    class SLM01 : Mine
    {
        public override int MineType => 7;

        public SLM01(int id, string hash, Vector pos, Spacemap map) : base(id, hash, pos, map)
        {
        }

        public override void Effect()
        {
            var area = Spacemap.Entities.Where(x => x.Value.Position.DistanceTo(Position) <= 1000 && x.Value is Player);
            foreach (var entry in area)
            {
                if (entry.Value.Cooldowns.Any(x => x is DecelerationEffect))
                {
                    var dc = entry.Value.Cooldowns.Cooldowns.Find(x => x is DecelerationEffect);
                    dc.EndTime = dc.EndTime.AddSeconds(3);
                }
                else
                {
                    var effect = new DecelerationEffect();
                    effect.OnStart(entry.Value);
                    entry.Value.Cooldowns.Add(effect);
                }
            }
        }
    }
}
