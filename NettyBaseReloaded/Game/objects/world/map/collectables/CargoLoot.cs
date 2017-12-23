using NettyBaseReloaded.Game.objects.world.characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class CargoLoot : Collectable
    {
        private Character Killer { get; }
        private DropableRewards Rewards { get; set; }
        public CargoLoot(int id, string hash, Vector pos, DropableRewards dropableRewards, Character killer) : base(id,hash,Types.SHIP_LOOT, pos)
        {
            Rewards = dropableRewards;
            Killer = killer;
            InitiateDisposalTimer();
        }
        
        private async void InitiateDisposalTimer()
        {
            var map = Killer?.Spacemap;
            if (map == null) return;
            await Task.Delay(30000);
            Dispose(map);
        }

        protected override void Reward(Player player)
        {
            Console.WriteLine("TODO: Cargobox reward");
        }

        public override int GetTypeId(Character character)
        {
            if (Killer == character) return (int)Types.SHIP_LOOT;
            else return (int)Types.SHIP_LOOT_GRAY;
        }
    }
}
