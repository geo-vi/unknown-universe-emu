using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.objects.world.map;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class Range
    {
        public ConcurrentDictionary<int, Character> Entities = new ConcurrentDictionary<int, Character>();
        public ConcurrentDictionary<int, Object> Objects = new ConcurrentDictionary<int, Object>();

        public Dictionary<string, Collectable> Collectables = new Dictionary<string, Collectable>();
        public Dictionary<string, Ore> Resources = new Dictionary<string, Ore>();
        public Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();
       
        public Character Character { get; set; }

        public Character GetEntity(int id)
        {
            return Entities.ContainsKey(id) ? Entities[id] : null;
        }

        public bool AddEntity(Character entity)
        {
            var success = Entities.TryAdd(entity.Id, entity);
            return success;
        }

        public bool RemoveEntity(Character entity)
        {
            Character output;
            var success = Entities.TryRemove(entity.Id, out output);
            return success;
        }

        public bool AddObject(Object obj)
        {
            var collectable = obj as Collectable;
            if (collectable != null)
            {
                if (!Collectables.ContainsKey(collectable.Hash))
                {
                    Collectables.Add(collectable.Hash, collectable);
                }
            }
            else if (obj is Ore ore)
            {
                if (!Resources.ContainsKey(ore.Hash))
                {
                    Resources.Add(ore.Hash, ore);
                }
            } 
            return Objects.TryAdd(obj.Id, obj);
        }

        public bool RemoveObject(Object obj)
        {
            var collectable = obj as Collectable;
            if (collectable != null)
            {
                Collectables.Remove(collectable.Hash);
            }
            return Objects.TryRemove(obj.Id, out obj);
        }

        public void Clean()
        {
            Entities.Clear();
            Objects.Clear();
            Collectables.Clear();
            Resources.Clear();
            Zones.Clear();
        }
    }
}
