using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.mines;

namespace NettyBaseReloaded.Game.objects.world.map.zones
{
    class MineZone : Zone
    {
        private Spacemap Map;

        public MineZone(int id, Vector topLeft, Vector botRight, Spacemap map) : base(id, topLeft, botRight, Faction.NONE)
        {
            Map = map;
        }

        public void Spawn(int amount = 150)
        {
            for (int i = 0; i < amount; i++)
            {
                var id = Map.GetNextObjectId();
                var hash = Map.HashedObjects.Keys.ToList()[id];
                Map.AddObject(new PM00(id, hash, Vector.Random(Map, TopLeft, BottomRight), Map));
            }
        }
    }
}
