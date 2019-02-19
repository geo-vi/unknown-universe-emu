using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.item
{
    static class UpgradeLevels
    {
        public static readonly Dictionary<int, double> DamageBoostUridium = new Dictionary<int, double>();
        public static readonly Dictionary<int, double> ShieldBoostUridium = new Dictionary<int, double>();
        public static readonly Dictionary<int, double> DamageBoostCredits = new Dictionary<int, double>();
        public static readonly Dictionary<int, double> ShieldBoostCredits = new Dictionary<int, double>();
    }
}
