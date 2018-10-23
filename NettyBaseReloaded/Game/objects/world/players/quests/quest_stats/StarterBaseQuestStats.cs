using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.quests.quest_stats
{
    class StarterBaseQuestStats
    {
        public int KilledStreuner;
        public bool Complete1;
        public int KilledLordakia;
        public bool Complete2;
        public bool Complete => Complete1 && Complete2;
    }
}
