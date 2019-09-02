using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Ammunition : PlayerBaseClass
    {
        public string LootId { get; set; }
        private int Amount { get; set; }
        private int SyncedAmount { get; set; }

        public DateTime LastSynchronizationTime = new DateTime();

        public Ammunition(Player player, string lootId, int amount) : base(player)
        {
            LootId = lootId;
            Amount = amount;
            SyncedAmount = amount;
            LastSynchronizationTime = DateTime.Now;
        }

        public int Get()
        {
            return Amount;
        }

        public int Shoot(string shooter = "ship")
        {
            int fireCount;
            switch (shooter)
            {
                case "ship":
                    if (LootId.Contains("ammunition_laser"))
                    {
                        fireCount = Player.Equipment.LaserCount();
                        if (fireCount > Amount) return 0;
                    }
                    else if (LootId.Contains("ammunition_rocket"))
                    {
                        fireCount = 1;
                        if (fireCount > Amount) return 0;
                    }
                    else fireCount = 1;

                    if (fireCount > Amount) return 0;

                    Add(-fireCount);
                    break;
                case "pet":
                    if (LootId.Contains("ammunition_laser"))
                    {
                        fireCount = Player.Equipment.LaserCount(true);
                        if (fireCount > Amount) return 0;
                        Player.Skylab.ReduceLaserOre(fireCount);
                    }
                    else fireCount = 1;

                    if (fireCount > Amount) return 0;

                    Add(-fireCount);
                    break;
                default:
                    fireCount = 0;
                    break;
            }
            return fireCount;
        }

        public void Add(int amount)
        {
            Amount += amount;
            SyncCheck(true);
            var session = World.StorageManager.GetGameSession(Player.Id);
            if (session != null)
            {
                Packet.Builder.AmmunitionCountUpdateCommand(session, LootId, Amount);
            }
        }

        private void SyncCheck(bool force = false)
        {
            if (LastSynchronizationTime.AddSeconds(15) < DateTime.Now || force)
            {
                SyncedAmount = World.DatabaseManager.UpdateAmmo(this, SyncedAmount - Amount);
                Amount = SyncedAmount;
                LastSynchronizationTime = DateTime.Now;
            }
        }

        public static void ForceSync(Player player)
        {
            foreach (var ammo in player.Information.Ammunitions)
            {
                ammo.Value.SyncCheck(true);
                Packet.Builder.AmmunitionCountUpdateCommand(World.StorageManager.GetGameSession(player.Id), ammo.Key,
                    ammo.Value.Amount);
            }
        }
    }
}
