using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestElement
    {
        public QuestRoot Case { get; set; }
        public QuestCondition Condition { get; set; }
    }
}
