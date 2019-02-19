using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.handlers;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
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

        public int SelectedQuestId = 0;

        public QuestPlayerData(Player player) : base(player)
        {
            Conditions = World.DatabaseManager.LoadPlayerQuestData(player);
            LoadCompletedQuests();
            SelectedQuestId = GetPrimaryQuest();
        }

        private int GetPrimaryQuest()
        {
            var activeCond = ActiveConditions.FirstOrDefault();
            if (activeCond.Value == null) return 0;
            return activeCond.Value.QuestId;
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

        public void Tick()
        {
            PositionMonitor();
        }

        public void AddKill(IAttackable attackable)
        {
            bool added = false;
            foreach (var activeCondition in ActiveConditions.Values)
            {
                var quest = FindQuestByState(activeCondition);
                var condition = quest.Root.Elements.Find(x => x.Condition.Id == activeCondition.ConditionId).Condition;
                var matches = condition.Matches;
                Npc npc = null;
                switch(condition.Type)
                {
                    case QuestConditions.KILL:
                        break;
                    case QuestConditions.KILL_ANY:
                        break;
                    case QuestConditions.KILL_NPC:
                        if (attackable is Npc)
                        {
                            npc = (Npc) attackable;
                            var matchId = condition.Matches[0];
                            if (World.StorageManager.NpcReferences.ContainsKey(matchId) && World.StorageManager.NpcReferences[matchId] == npc.Hangar.Ship && !added)
                            {
                                if (activeCondition.CurrentValue + 1 == condition.TargetValue && condition.Mandatory)
                                {
                                    activeCondition.Completed = true;
                                    activeCondition.Active = false;
                                }
                                activeCondition.CurrentValue++;
                                Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), activeCondition);
                                World.DatabaseManager.UpdateQuestCondition(Player, activeCondition);
                                added = true;
                            }
                        }
                        break;
                    case QuestConditions.KILL_NPCS:
                        if (attackable is Npc)
                        {
                            npc = (Npc) attackable;

                            bool add = false;
                            for (var i = 1; i < matches.Count; i++)
                            {
                                var matchId = matches[i];
                                if (World.StorageManager.NpcReferences.ContainsKey(matchId) && World.StorageManager.NpcReferences[matchId] == npc.Hangar.Ship && !added)
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
                                Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), activeCondition);
                                World.DatabaseManager.UpdateQuestCondition(Player, activeCondition);
                                added = true;
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
                                if (World.StorageManager.ShipReferences.ContainsKey(matchId) && World.StorageManager.ShipReferences[matchId] == player.Hangar.Ship && !added)
                                {
                                    if (matches.Count > 2)
                                    {
                                        var targetFac = matches[2];
                                        if (attackable.FactionId == (Faction) targetFac)
                                            add = true;
                                    }
                                    else add = true;
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
                                Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), activeCondition);
                                World.DatabaseManager.UpdateQuestCondition(Player, activeCondition);
                                added = true;
                            }
                        }
                        break;
                }
                if (quest.IsComplete(Conditions))
                {
                    CompleteQuest(quest);
                }
            }
        }

        public void AddCollection(Collectable collectable)
        {
            try
            {
                bool added = false;
                foreach (var activeCondition in ActiveConditions.Values)
                {
                    if (activeCondition.QuestId != SelectedQuestId) continue;
                    var quest = FindQuestByState(activeCondition);
                    var condition = quest.Root.Elements.Find(x => x.Condition.Id == activeCondition.ConditionId)
                        .Condition;
                    if (condition.Type == QuestConditions.COLLECT_BONUS_BOX && !added)
                    {
                        if (activeCondition.CurrentValue + 1 == condition.TargetValue)
                        {
                            activeCondition.Completed = true;
                            activeCondition.Active = false;
                        }

                        activeCondition.CurrentValue++;
                        Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), activeCondition);
                        World.DatabaseManager.UpdateQuestCondition(Player, activeCondition);
                        added = true;
                    }
                    if (quest.IsComplete(Conditions))
                    {
                        CompleteQuest(quest);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + ":Collection\n" + e.Message);
            }
        }

        public void AddResourceCollection(Ore ore)
        {
            try
            {
                bool added = false;
                foreach (var activeCondition in ActiveConditions.Values)
                {
                    if (activeCondition.QuestId != SelectedQuestId) continue;
                    var quest = FindQuestByState(activeCondition);
                    var condition = quest.Root.Elements.Find(x => x.Condition.Id == activeCondition.ConditionId)
                        .Condition;
                    if (condition.Type == QuestConditions.COLLECT && condition.Matches[0] == (int) ore.Type + 1 && !added)
                    {
                        if (activeCondition.CurrentValue + 1 == condition.TargetValue)
                        {
                            activeCondition.Completed = true;
                            activeCondition.Active = false;
                        }

                        activeCondition.CurrentValue++;
                        Packet.Builder.QuestConditionUpdateCommand(Player.GetGameSession(), activeCondition);
                        World.DatabaseManager.UpdateQuestCondition(Player, activeCondition);
                        added = true;
                    }
                    if (quest.IsComplete(Conditions))
                    {
                        CompleteQuest(quest);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + ":Resource Collection\n" + e.Message);
            }
        }

        /// <summary>
        /// If Position monitor is needed just override Tick and add PositionMonitor - (saving resources).
        /// </summary>
        private void PositionMonitor()
        {
            foreach (var activeCondition in ActiveConditions.Values)
            {
                if (activeCondition.QuestId != SelectedQuestId) continue;
                var quest = FindQuestByState(activeCondition);
                var condition = quest.Root.Elements.Find(x => x.Condition.Id == activeCondition.ConditionId).Condition;
                switch (condition.Type)
                {
                        case QuestConditions.VISIT_MAP:
                            var mapID = condition.Matches[0];
                            if (Player.Spacemap.Id == mapID)
                            {
                                activeCondition.Completed = true;
                                activeCondition.Active = false;
                            }
                            break;
                        case QuestConditions.VISIT_QUEST_GIVER:
                            if (Player.Range.Objects.Any(x => x.Value is QuestGiver))
                            {
                                activeCondition.Completed = true;
                                activeCondition.Active = false;
                            }
                            break;
                        case QuestConditions.COORDINATES:
                            //todo
                            break;
                }
            }
        }

        private Quest FindQuestByState(QuestSerializableState state)
        {
            return World.StorageManager.Quests[state.QuestId];
        }

        private Quest FindQuestByCondition(int conditionId)
        {
            var quest = World.StorageManager.Quests.FirstOrDefault(x => x.Value.Root.FindElement(conditionId) != null).Value;
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
            Packet.Builder.QuestInitializationCommand(Player.GetGameSession());
            if (SelectedQuestId == 0) SelectedQuestId = quest.Id;
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
            if (CompletedQuests.ContainsKey(quest.Id)) return;
            quest.Reward.ParseRewards(Player, (int)(Player.BoostedQuestReward + 1));
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