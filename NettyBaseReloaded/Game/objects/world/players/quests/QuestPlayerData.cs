using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.players.quests.serializables;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestPlayerData : PlayerBaseClass
    {
        private Dictionary<int, QuestSerializableState> Conditions;
        
        public Dictionary<int, Quest> CompletedQuests = new Dictionary<int, Quest>();
        
        public QuestPlayerData(Player player) : base(player)
        {
            Conditions = World.DatabaseManager.LoadPlayerQuestData(player);
            LoadCompletedQuests();
        }

        private void LoadCompletedQuests()
        {
            foreach (var quest in World.StorageManager.Quests)
            {
                if (quest.Value.IsComplete(Conditions)) CompletedQuests.Add(quest.Key, quest.Value);
            }
        }
        
        public virtual void Tick() { }

        public virtual void AddKill(IAttackable attackable)
        {
            
        }

        public virtual void AddCollection(Collectable collectable)
        {
        }

        public virtual void AddResourceCollection(Ore ore)
        {
        }

        /// <summary>
        /// If Position monitor is needed just override Tick and add PositionMonitor - (saving resources).
        /// </summary>
        public void PositionMonitor()
        {

        }
    }
}