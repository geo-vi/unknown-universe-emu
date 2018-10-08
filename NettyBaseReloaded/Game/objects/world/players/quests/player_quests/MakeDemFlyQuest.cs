using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.quests.quest_stats;

namespace NettyBaseReloaded.Game.objects.world.players.quests.player_quests
{
    class MakeDemFlyQuest : Quest
    {
        public override QuestIcons Icon => QuestIcons.PVP;

        public override QuestTypes QuestType => QuestTypes.MISSION;

        public override QuestRoot Root => new QuestRoot
        {
            Active = false,
            Elements = new List<QuestElement>
            {
                new QuestElement
                {
                    Case = new QuestRoot
                    {
                        Active = false,
                        Elements = new List<QuestElement>(),
                        Id = 0,
                        Mandatory = false,
                        MandatoryCount = 0,
                        Ordered = false
                    },
                    Condition = new QuestCondition
                    {
                        Id = 10301,
                        Mandatory = true,
                        Matches = new List<int>{1, 15},
                        State = new QuestState { Active = true, Completed = Stats.Completed, CurrentValue = Stats.Damage},
                        SubConditions = new List<QuestCondition>(),
                        Type = QuestConditions.KILL_NPCS,
                        TargetValue = 5
                    }
                },
                new QuestElement
                {
                    Case = new QuestRoot
                    {
                        Active = false,
                        Elements = new List<QuestElement>(),
                        Id = 0,
                        Mandatory = false,
                        MandatoryCount = 0,
                        Ordered = false
                    },
                    Condition = new QuestCondition
                    {
                        Id = 10302,
                        Mandatory = false,
                        Matches = new List<int>(),
                        State = new QuestState { Active = true, Completed = Stats.Completed, CurrentValue = Stats.Damage},
                        SubConditions = new List<QuestCondition>(),
                        Type = QuestConditions.AVOID_DEATH,
                        TargetValue = 0
                    }
                }
            },
            Id = 10300,
            Mandatory = true,
            MandatoryCount = 2,
            Ordered = false
        };

        public override Reward Reward => new Reward(new Dictionary<RewardType, int>
        {
            {RewardType.CREDITS, 255000 },
            {RewardType.URIDIUM, 2500 },
            {RewardType.EXPERIENCE, 128000 },
            {RewardType.HONOR, 8128 },
        });

        public MakeDemFlyQuestStats Stats = new MakeDemFlyQuestStats();

        public MakeDemFlyQuest(Player player, MakeDemFlyQuestStats stat) : base(player, 3)
        {
            Stats = stat;
        }
    }
}
