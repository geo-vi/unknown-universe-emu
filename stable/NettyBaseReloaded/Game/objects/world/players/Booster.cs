using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Booster
    {
        public enum Types
        {
            DAMAGE_BOOSTER_NORMAL,
            SHIELD_BOOSTER_NORMAL,
            DAMAGE_BOOSTER_EXTRA,
            SHIELD_BOOSTER_EXTRA,

        }
        public Types Type { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }

        public Booster(Types type, DateTime startTime, DateTime expireTime)
        {
            Type = type;
            StartTime = startTime;
            ExpireTime = expireTime;
        }

        public int GetTimeInMins()
        {
            return (ExpireTime = StartTime).Minute;
        }

        public double GetBonus()
        {
            switch (Type)
            {
                case Types.DAMAGE_BOOSTER_NORMAL:
                case Types.SHIELD_BOOSTER_NORMAL:
                    return 1.10;
                case Types.DAMAGE_BOOSTER_EXTRA:
                case Types.SHIELD_BOOSTER_EXTRA:
                    return 1.10;
                default:
                    return 1;
            }
        }
    }
}
