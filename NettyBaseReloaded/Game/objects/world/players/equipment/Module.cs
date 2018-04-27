using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class Module
    {
        public enum Types
        {
            NONE = 0,
            DESTROYED = 1,
            HULL = 2,
            DEFLECTOR = 3,
            REPAIR = 4,
            LASER_HIGH_RANGE = 5,
            LASER_MID_RANGE = 6,
            LASER_LOW_RANGE = 7,
            ROCKET_MID_ACCURACY = 8,
            ROCKET_LOW_ACCURACY = 9,
            HONOR_BOOSTER = 10,
            DAMAGE_BOOSTER = 11,
            EXPERIENCE_BOOSTER = 12
        }

        public Types ModuleType { get; set; }

        public Item Item { get; set; }

        public int EquippedBattleStationId;
        public bool Equipped { get; set; }

        public bool Destroyed => BattleStationModule?.Core.CurrentHealth <= 0;

        public BattleStationModule BattleStationModule { get; set; }

        public Module(Types moduleType, Item item, bool equipped)
        {
            ModuleType = moduleType;
            Item = item;
            Equipped = equipped;
        }

        public BattleStationModule Find(Player player)
        {
            if (Equipped)
            {
                if (BattleStationModule != null && BattleStationModule.Owner == player &&
                    BattleStationModule.SlotId != -1) return BattleStationModule;
                Equipped = false;
            }

            return null;
        }
    }
}
