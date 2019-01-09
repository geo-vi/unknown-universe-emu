using System;
using QuestBuilder.quests.serializables;

namespace QuestBuilder.quests
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
    }
}