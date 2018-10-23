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
    class StarterBaseQuest : Quest
    {
        public override QuestIcons Icon => QuestIcons.KILL;

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
                        MandatoryCount = 1,
                        Ordered = false
                    },
                    Condition = new QuestCondition
                    {
                        Id = 10201,
                        Mandatory = false,
                        Matches = new List<int> { 2, 1, 14 },
                        State = new QuestState { Active = true, Completed = Stats.Complete1, CurrentValue = Stats.KilledStreuner},
                        SubConditions = new List<QuestCondition>(),
                        Type = QuestConditions.KILL_NPCS,
                        TargetValue = 50
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
                        Id = 10202,
                        Mandatory = false,
                        Matches = new List<int> { 2, 2, 15},
                        State = new QuestState { Active = true, Completed = Stats.Complete2, CurrentValue = Stats.KilledLordakia},
                        SubConditions = new List<QuestCondition>(),
                        Type = QuestConditions.KILL_NPCS,
                        TargetValue = 50
                    }
                }
            },
            Id = 10200,
            Mandatory = false,
            MandatoryCount = 0,
            Ordered = false
        };

        public override Reward Reward => new Reward(new Dictionary<RewardType, int>
        {
            {RewardType.CREDITS, 255000 },
            {RewardType.URIDIUM, 10000 },
            {RewardType.EXPERIENCE, 128000 },
            {RewardType.HONOR, 8128 }
        });

        public StarterBaseQuestStats Stats = new StarterBaseQuestStats();

        public StarterBaseQuest(Player player, StarterBaseQuestStats stat) : base(player, 2)
        {
            Stats = stat;
        }

        public override void AddKill(IAttackable attackable)
        {
            if (attackable is Character deadCharacter && deadCharacter.FactionId != Player.FactionId)
            {
                if (deadCharacter.Hangar.Ship.RootId == 84 || deadCharacter.Hangar.Ship.RootId == 23)
                {
                    if (Stats.KilledStreuner >= 50)
                    {
                        Stats.Complete1 = true;
                    }
                    else Stats.KilledStreuner += 1;
                }
                else if (deadCharacter.Hangar.Ship.RootId == 71 || deadCharacter.Hangar.Ship.RootId == 24)
                {
                    if (Stats.KilledLordakia >= 50)
                    {
                        Stats.Complete2 = true;
                    }
                    else Stats.KilledLordakia += 1;
                }

                foreach (var element in Root.Elements)
                {
                    Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), element.Condition);
                }

                if (Stats.Complete1 && Stats.Complete2)
                {
                    Packet.Builder.QuestCompletedCommand(Player.GetGameSession(), this);
                    Player.AcceptedQuests.Remove(this);
                    Player.CompletedQuests.Add(this);
                    Packet.Builder.QuestListCommand(Player.GetGameSession());
                    Reward.ParseRewards(Player);
                }
            }
            base.AddKill(attackable);
        }

    }
}
