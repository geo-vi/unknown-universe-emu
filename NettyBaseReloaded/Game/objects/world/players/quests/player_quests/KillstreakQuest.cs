using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.players.quests.player_quests
{
    class KillstreakQuest : Quest
    {
        public override QuestIcons Icon => QuestIcons.PVP;

        public override QuestTypes QuestType => QuestTypes.MISSION;

        public override QuestRoot Root => new QuestRoot
        {
            Active = true,
            Elements = new List<QuestElement>
            {
                new QuestElement
                {
                    Case = new QuestRoot
                    {
                        Active = true,
                        Elements = new List<QuestElement>(),
                        Id = 0,
                        Mandatory = false,
                        MandatoryCount = 1,
                        Ordered = false
                    },
                    Condition = new QuestCondition
                    {
                        Id = 10001,
                        Mandatory = false,
                        Matches = new List<int> { 1,2,3,10,1 },
                        State = new QuestState { Active = true, Completed = false},
                        SubConditions = new List<QuestCondition>(),
                        Type = QuestConditions.KILL_PLAYERS,
                        TargetValue = 10
                    }
                }
            },
            Id = 10000,
            Mandatory = true,
            MandatoryCount = 0,
            Ordered = false
        };

        public override Reward Reward => new Reward(new Dictionary<RewardType, int>
        {
            {RewardType.CREDITS, 255000 },
            {RewardType.URIDIUM, 5000 },
            {RewardType.EXPERIENCE, 128000 },
            {RewardType.HONOR, 8128 }
        });
        
        public KillstreakQuest(Player player) : base(player, 0)
        {
            Root.LoadPlayerData(player);
        }
    }
}
