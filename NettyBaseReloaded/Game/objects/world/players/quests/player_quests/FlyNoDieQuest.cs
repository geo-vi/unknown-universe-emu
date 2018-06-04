using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players.quests.quest_stats;

namespace NettyBaseReloaded.Game.objects.world.players.quests.player_quests
{
    class FlyNoDieQuest : Quest
    {
        public override QuestIcons Icon => QuestIcons.DISCOVER;

        public override QuestTypes QuestType => QuestTypes.MISSION;

        public override QuestRoot Root => new QuestRoot
        {
            Active = true,
            Elements = new List<QuestElement>
            {
                new QuestElement
                {
                    Case = new QuestRoot(),
                    Condition = new QuestCondition()
                }
            },
            Id = 10010,
            Mandatory = false,
            MandatoryCount = 0,
            Ordered = true
        };

        public FlyNoDieQuestStats Stats = new FlyNoDieQuestStats();

        public FlyNoDieQuest(Player player, FlyNoDieQuestStats stats) : base(player, 1)
        {
            Stats = stats;
        }
    }
}
