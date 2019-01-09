using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.objects.world.map.ores
{
    class RegularOre : Ore
    {
        public RegularOre(int id, string hash, OreTypes type, Vector pos, Spacemap map, Vector[] limits) : base(id, hash, type, pos, map, limits)
        {
        }

        public override void Collect(Character character)
        {
            base.Collect(character);
            Reward(character as Player);
        }

        private void Reward(Player player)
        {
            if (player == null) return;

            switch (Type)
            {
                case OreTypes.PROMETIUM:
                    player.Information.Cargo.TryAdd(0, 1);
                    break;
                case OreTypes.ENDURIUM:
                    player.Information.Cargo.TryAdd(1, 1);
                    break;
                case OreTypes.TERBIUM:
                    player.Information.Cargo.TryAdd(2, 1);
                    break;
            }
        }
    }
}
