using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.quests;
using NettyBaseReloaded.Game.objects.world.players.quests.serializables;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Quest
    {     
        public int Id { get; set; }

        public QuestTypes QuestType { get; private set; }

        public QuestRoot Root { get; private set; }

        public QuestIcons Icon { get; private set; }

        public Reward Reward { get; private set; }

        public bool Canceled;

        public Quest(int id)
        {
            Id = id;
        }

        public QuestAcceptability GetAcceptabilityStatus(Player player)
        {
            if (player.QuestData.IsQuestActive(Id)) return QuestAcceptability.RUNNING;
            if (player.QuestData.CompletedQuests.ContainsKey(Id))
                return QuestAcceptability.COMPLETED;
            return QuestAcceptability.NOT_STARTED;
        }

        public List<netty.commands.old_client.LootModule> GetOldLootModule()
        {
            List<LootModule> rewardsList = new List<LootModule>();
            RewardType typeOfReward = RewardType.CREDITS;
            Item item = null;
            int amount = 0;
            foreach (var reward in Reward.Rewards)
            {
                if (reward is RewardType _typeOfReward)
                {
                    typeOfReward = _typeOfReward;
                }
                else if (reward is Item _item)
                {
                    item = _item;
                }
                else if (reward is int _amount)
                {
                    amount = _amount;
                    if (amount == 0) continue;
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
                        case RewardType.ITEM:
                            rewardsList.Add(new LootModule(item.LootId, amount));
                            break;
                    }
                }
                
            }

            return rewardsList;

        }

        public void SetReward(Reward reward)
        {
            Reward = reward;
        }

        public void SetType(QuestTypes type)
        {
            QuestType = type;
        }

        public void SetRoot(QuestRoot root)
        {
            Root = root;
        }

        public void SetIcon(QuestIcons icon)
        {
            Icon = icon;
        }

        public bool IsComplete(ConcurrentDictionary<int, QuestSerializableState> conditions)
        {
            bool completed = false;
            foreach (var element in Root.Elements)
            {
                if (conditions.ContainsKey(element.Condition.Id))
                {
                    completed = conditions[element.Condition.Id].Completed;
                }
                else completed = false;
            }

            return completed;
        }
    }
}
