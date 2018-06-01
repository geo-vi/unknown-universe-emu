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
                        Matches = new List<int> { 2,8,10 },
                        State = new QuestState { Active = true, Completed = Stats.Complete, CurrentValue = Stats.Kills},
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
        
        public KillstreakQuestStat Stats = new KillstreakQuestStat();

        public KillstreakQuest(Player player, KillstreakQuestStat stat) : base(player, 0)
        {
            Stats = stat;
        }

        public override void AddKill(IAttackable attackable)
        {
            if (attackable is Character deadCharacter && deadCharacter.FactionId != Player.FactionId &&
                (deadCharacter.Hangar.Ship.RootId == 8 || deadCharacter.Hangar.Ship.RootId == 10))
            {
                Stats.Kills += 1;
                if (Stats.Kills == 10)
                {
                    Stats.Complete = true;
                    Packet.Builder.QuestCompletedCommand(Player.GetGameSession(), this);
                    Player.AcceptedQuests.Remove(this);
                    Player.CompletedQuests.Add(this);
                    Packet.Builder.QuestListCommand(Player.GetGameSession());
                    Reward.ParseRewards(Player);
                }
                Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), Root.Elements[0].Condition);
            }
            base.AddKill(attackable);
        }
    }
}
