using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.players.quests;
using NettyBaseReloaded.Game.objects.world.players.quests.player_quests;
using NettyBaseReloaded.Game.objects.world.players.quests.quest_stats;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Quest : PlayerBaseClass
    {
        #region Static

        public static List<Quest> Quests = new List<Quest>
        {
            new KillstreakQuest(null, new KillstreakQuestStat()),
            //new FlyNoDieQuest(null, new FlyNoDieQuestStats()),
            new StarterBaseQuest(null, new StarterBaseQuestStats()) ,
            //new MakeDemFlyQuest(null, new MakeDemFlyQuestStats())
        };


        #endregion

        public int Id { get; set; }

        public virtual QuestTypes QuestType { get; }

        public virtual QuestRoot Root { get; }

        public virtual QuestIcons Icon { get; }

        public virtual Reward Reward { get; }

        public bool Canceled;

        public Quest(Player player, int id) : base(player)
        {
            Id = id;
            Root.LoadPlayerData(player);
        }

        public virtual void Tick() { }

        public virtual void AddKill(IAttackable attackable)
        {
            World.DatabaseManager.SaveQuestData(Player, this);
        }

        public virtual void AddCollection(Collectable collectable)
        {
            World.DatabaseManager.SaveQuestData(Player, this);
        }

        public virtual void AddResourceCollection(Ore ore)
        {
            World.DatabaseManager.SaveQuestData(Player, this);
        }

        /// <summary>
        /// If Position monitor is needed just override Tick and add PositionMonitor - (saving resources).
        /// </summary>
        public void PositionMonitor()
        {

        }

        public QuestAcceptability GetAcceptabilityStatus(Player player)
        {
            if (player.AcceptedQuests.Exists(x => x.GetType() == GetType()))
                return QuestAcceptability.RUNNING;
            
            if (player.CompletedQuests.Exists(x => x.GetType() == GetType()))
                return QuestAcceptability.COMPLETED;

            return QuestAcceptability.NOT_STARTED;
        }

        public List<netty.commands.old_client.LootModule> GetOldLootModule()
        {
            List<LootModule> rewardsList = new List<LootModule>();
            RewardType typeOfReward = RewardType.HONOR;
            string lootId = "";
            int amount = 0;
            foreach (var reward in Reward.Rewards)
            {
                if (reward is RewardType _typeOfReward)
                {
                    typeOfReward = _typeOfReward;
                }
                else if (reward is string _lootId)
                {
                    lootId = _lootId;
                }
                else if (reward is int _amount)
                {
                    amount = _amount;
                    switch (typeOfReward)
                    {
                        case RewardType.CREDITS:
                            rewardsList.Add(new LootModule("currency_credits", amount));
                            break;
                        case RewardType.URIDIUM:
                            rewardsList.Add(new LootModule("currency_uridium", amount));
                            break;
                        case RewardType.EXPERIENCE:
                            rewardsList.Add(new LootModule("currency_experience", amount));
                            break;
                        case RewardType.HONOR:
                            rewardsList.Add(new LootModule("currency_honour", amount));
                            break;
                    }
                }
                
            }

            return rewardsList;

        }
    }
}
