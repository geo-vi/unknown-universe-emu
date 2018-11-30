using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.players.quests.serializables;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestPlayerData : PlayerBaseClass
    {
        private ConcurrentDictionary<int, QuestSerializableState> Conditions;
        
        /// <summary>
        /// Loaded from within Quests
        /// </summary>
        public readonly Dictionary<int, Quest> CompletedQuests = new Dictionary<int, Quest>();
        
        /// <summary>
        /// Currently active quest conds
        /// </summary>
        public Dictionary<int, QuestSerializableState> ActiveConditions
        {
            get
            {
                var conds = Conditions.Where(x => x.Value.Active);
                return conds.ToDictionary(x => x.Key, y => y.Value);
            }
        }

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

        public QuestSerializableState GetData(int conditionId)
        {
            if (Conditions.ContainsKey(conditionId))
            {
                return Conditions[conditionId];
            }
            return new QuestSerializableState() { ConditionId = conditionId, QuestId = FindQuestByCondition(conditionId).Id };
        }

        public virtual void Tick() { }

        public virtual void AddKill(IAttackable attackable)
        {
            foreach (var activeCondition in ActiveConditions.Values)
            {
                var quest = FindQuestByState(activeCondition);
                var condition = quest.Root.Elements.Find(x => x.Condition.Id == activeCondition.ConditionId).Condition;
                var matches = condition.Matches;
                switch(condition.Type)
                {
                    case QuestConditions.KILL:
                        break;
                    case QuestConditions.KILL_ANY:
                        break;
                    case QuestConditions.KILL_NPC:
                        break;
                    case QuestConditions.KILL_NPCS:
                        if (attackable is Npc npc)
                        {
                            bool add = false;
                            for (var i = 1; i < matches.Count; i++)
                            {
                                var matchId = matches[i];
                                if (World.StorageManager.NpcReferences.ContainsKey(matchId) && World.StorageManager.NpcReferences[matchId] == npc.Hangar.Ship)
                                {
                                    add = true;
                                }
                            }
                            if (add)
                            {
                                if (activeCondition.CurrentValue + 1 == condition.TargetValue)
                                {
                                    activeCondition.Completed = true;
                                    activeCondition.Active = false;
                                }
                                activeCondition.CurrentValue++;
                            }
                        }
                        break;
                    case QuestConditions.KILL_PLAYER:
                        break;
                    case QuestConditions.KILL_PLAYERS:
                        if (attackable is Player player)
                        {
                            bool add = false;
                            for (var i = 1; i < matches.Count; i++)
                            {
                                var matchId = matches[i];
                                if (World.StorageManager.ShipReferences.ContainsKey(matchId) && World.StorageManager.ShipReferences[matchId] == player.Hangar.Ship)
                                {
                                    add = true;
                                }
                            }
                            if (add)
                            {
                                if (activeCondition.CurrentValue + 1 == condition.TargetValue)
                                {
                                    activeCondition.Completed = true;
                                    activeCondition.Active = false;
                                }

                                activeCondition.CurrentValue++;
                            }
                        }
                        break;
                }
                Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), activeCondition);
                if (Conditions.All(x => x.Value.Completed && x.Value.QuestId == quest.Id))
                {
                    CompleteQuest(quest);
                }
                World.DatabaseManager.UpdateQuestCondition(Player, activeCondition);
            }
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

        private Quest FindQuestByState(QuestSerializableState state)
        {
            return World.StorageManager.Quests[state.QuestId];
        }

        private Quest FindQuestByCondition(int conditionId)
        {
            var quest = World.StorageManager.Quests.FirstOrDefault(x => x.Value.Root.Elements.FirstOrDefault(y => y.Condition.Id == conditionId) != null).Value;
            return quest;
        }

        public bool IsQuestActive(int questId)
        {
            if (ActiveConditions.Values.FirstOrDefault(x => x.QuestId == questId) != null) return true;
            return false;
        }

        public void AcceptQuest(Quest quest)
        {
            foreach (var element in quest.Root.Elements)
            {
                var state = new QuestSerializableState() { ConditionId = element.Condition.Id, QuestId = quest.Id };
                if (quest.Root.Mandatory && element.Case.MandatoryCount == 0)
                {
                    state.Active = true;
                }
                else if (!quest.Root.Mandatory) state.Active = true;
                Conditions.TryAdd(element.Condition.Id, state);
                World.DatabaseManager.AddQuestCondition(Player, state);
            }
            Console.WriteLine(JsonConvert.SerializeObject(Conditions));
            Packet.Builder.QuestInitializationCommand(Player.GetGameSession());
        }

        public List<Quest> GetActiveQuests()
        {
            List<Quest> questList = new List<Quest>();
            foreach (var activeCond in ActiveConditions)
            {
                var q = FindQuestByState(activeCond.Value);
                if (!questList.Contains(q))
                    questList.Add(q);
            }
            return questList;
        }

        public void CompleteQuest(Quest quest)
        {
            quest.Reward.ParseRewards(Player);
            CompletedQuests.Add(quest.Id, quest);
            Packet.Builder.QuestCompletedCommand(Player.GetGameSession(), quest);
        }

        public void CancelQuest(Quest quest)
        {
            var conds = Conditions.Values.Where(x => x.QuestId == quest.Id).ToList();
            foreach (var condition in conds)
            {
                QuestSerializableState removedState;
                if (!Conditions.TryRemove(condition.ConditionId, out removedState))
                {
                    return;
                }
            }
            World.DatabaseManager.RemovePlayerQuest(Player, quest.Id);
            Packet.Builder.QuestCancelledCommand(Player.GetGameSession(), quest.Id);
        }
    }
}