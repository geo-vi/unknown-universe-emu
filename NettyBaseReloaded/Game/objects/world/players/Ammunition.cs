using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Ammunition : PlayerBaseClass
    {
        public string LootId { get; set; }
        private int Amount { get; set; }

        internal DateTime LastSynchronizationTime = new DateTime();

        public Ammunition(Player player, string lootId, int amount) : base(player)
        {
            LootId = lootId;
            Amount = amount;
        }

        public int Get()
        {
            if (LastSynchronizationTime.AddSeconds(30) < DateTime.Now)
            {
                // Sync
            }
            return Amount;
        }

        public int Shoot()
        {
            var newAmount = Amount - Player.Equipment.LaserCount();
            if (newAmount < 0)
            {
                return 0;
            }
            Amount = newAmount;
            return Player.Equipment.LaserCount();
        }

        public void Add(int amount)
        {
            
        }

        private void Sync()
        {
            
        }
    }
}
