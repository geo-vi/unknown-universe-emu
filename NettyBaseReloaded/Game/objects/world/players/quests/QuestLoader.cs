using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.quests.serializables;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestLoader
    {
        public int Id;
        /// <summary>
        /// Conditions
        /// </summary>
        public QuestRoot Root;
        /// <summary>
        /// Rewards
        /// </summary>
        public QuestSerializableReward Rewards;
        /// <summary>
        /// Quest Types as ENUM
        /// </summary>
        public QuestTypes QuestType;

        /// <summary>
        /// Quest Icon
        /// </summary>
        public QuestIcons Icon;
        
        /// <summary>
        /// Determines Quest's expiry, will disappear as soon as it reaches
        /// </summary>
        public DateTime ExpireDate;
        /// <summary>
        /// Used for daily quests to determine which day of the week this quest is going to be
        /// </summary>
        public int DayOfWeek;

        public static Quest Load(QuestLoader loader)
        {
            var quest = new Quest(loader.Id);

            quest.SetIcon(loader.Icon);
            
            quest.SetType(loader.QuestType);
            
            QuestRoot root = loader.Root;
            quest.SetRoot(root);

            var reward = new Reward(new Dictionary<RewardType, int>
            {
                {RewardType.CREDITS, loader.Rewards.Credits},
                {RewardType.URIDIUM, loader.Rewards.Uridium},
                {RewardType.EXPERIENCE, loader.Rewards.Exp},
                {RewardType.HONOR, loader.Rewards.Honor},
            });

            if (!string.IsNullOrEmpty(loader.Rewards.LootId))
            {
                var item = Item.Find(loader.Rewards.LootId);
                reward.Rewards.Add((short)0);
                reward.Rewards.Add(RewardType.ITEM);
                reward.Rewards.Add(item);
                reward.Rewards.Add(loader.Rewards.Amount);
            }
            quest.SetReward(reward);
            return quest;
        }
    }
}