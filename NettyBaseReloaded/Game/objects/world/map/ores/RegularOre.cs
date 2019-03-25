using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
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
            Player player = null;
            if (character is Pet pet)
            {
                player = pet.GetOwner();
            }
            else if (character is Player _player)
            {
                player = _player;
            }
            Reward(player);
        }

        private void Reward(Player player)
        {
            if (player == null) return;

            switch (Type)
            {
                case OreTypes.PROMETIUM:
                    player.Information.Cargo.Reward(new DropableRewards(1, 0,0,0,0,0,0,0,0));
                    break;
                case OreTypes.ENDURIUM:
                    player.Information.Cargo.Reward(new DropableRewards(0, 1, 0, 0, 0, 0, 0, 0, 0));
                    break;
                case OreTypes.TERBIUM:
                    player.Information.Cargo.Reward(new DropableRewards(0, 0, 1, 0, 0, 0, 0, 0, 0));
                    break;
                case OreTypes.PALLADIUM:
                    player.Information.Cargo.Reward(new DropableRewards(0, 0, 0, 0, 0, 0, 0, 0, 1));
                    break;
            }
        }
    }
}
