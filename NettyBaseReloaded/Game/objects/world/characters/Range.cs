using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.objects.world.map;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class Range
    {
        private Dictionary<int, Character> EntitiesPendingToBeAdded = new Dictionary<int, Character>();
        private Dictionary<int, Character> EntitiesPendingRemoval = new Dictionary<int, Character>();

        public Dictionary<int, Character> Entities = new Dictionary<int, Character>();
        public Dictionary<string, Collectable> Collectables = new Dictionary<string, Collectable>();
        public Dictionary<string, Ore> Resources = new Dictionary<string, Ore>();
        public Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();
        public Dictionary<int, Object> Objects = new Dictionary<int, Object>();

        public Character Character { get; set; }

        public Character GetEntity(int id)
        {
            return Entities.ContainsKey(id) ? Entities[id] : null;
        }

        public bool AddEntity(Character entity)
        {
            if (!EntitiesPendingToBeAdded.ContainsKey(entity.Id) && !Entities.ContainsKey(entity.Id))
            {
                EntitiesPendingToBeAdded.Add(entity.Id, entity);
                return true;
            }
            return false;
        }

        public bool RemoveEntity(Character entity)
        {
            if (!EntitiesPendingRemoval.ContainsKey(entity.Id) && Entities.ContainsKey(entity.Id))
            {
                EntitiesPendingRemoval.Add(entity.Id, entity);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the dictionary
        /// Adds / Removes pendings
        /// </summary>
        private void UpdateDictionary()
        {
            try
            {
                if (EntitiesPendingToBeAdded.Count > 0)
                {
                    foreach (var entity in EntitiesPendingToBeAdded.ToList())
                    {
                        if (!Entities.ContainsKey(entity.Key))
                            Entities.Add(entity.Key, entity.Value);
                    }
                    EntitiesPendingToBeAdded.Clear();
                }
                if (EntitiesPendingRemoval.Count > 0)
                {
                    foreach (var entity in EntitiesPendingRemoval.ToList())
                    {
                        if (Entities.ContainsKey(entity.Key))
                            Entities.Remove(entity.Key);
                    }
                    EntitiesPendingRemoval.Clear();
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("range_update", "Failed updating dictionaries", e);
            }
        }

        /// <summary>
        /// Validates if the spacemap in range are actually on the same map
        /// </summary>
        public void ValidateDictionary()
        {
            foreach (var entity in Entities.ToList())
            {
                if (entity.Value?.Spacemap == null)
                {
                    EntitiesPendingRemoval.Add(entity.Key, entity.Value);
                    continue;
                }

                if (entity.Value.Spacemap != Character.Spacemap) // TODO->Add Virtual Worlds
                    Character.Controller.Checkers.Update(entity.Value);
            }
        }

        public void Tick()
        {
            UpdateDictionary();
            ValidateDictionary();
        }

        public void Clear()
        {
            EntitiesPendingToBeAdded.Clear();
            EntitiesPendingRemoval.Clear();
            Entities.Clear();
            Collectables.Clear();
            Resources.Clear();
            Zones.Clear();
            Objects.Clear();
        }
    }
}
