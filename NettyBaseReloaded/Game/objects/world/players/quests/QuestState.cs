using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestState
    {
        public double CurrentValue { get; set; }
        public bool Active { get; set; }
        public bool Completed { get; set; }
    }
}
