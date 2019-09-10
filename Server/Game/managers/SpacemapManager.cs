using System;
using System.Collections.Concurrent;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;
using Server.Utils;
using Hangar = Server.Game.objects.entities.Hangar;

namespace Server.Game.managers
{
    class SpacemapManager
    {
        private static SpacemapManager _instance;
        
        public static SpacemapManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SpacemapManager();
                }
                return _instance;
            }
        }

        public int GetNextEntityId(Spacemap map)
        {
            return -1;
        }

        public void CreateNpc(Spacemap map, BaseNpc npcs)
        {
            for (var i = 0; i < npcs.Count; i++)
            {
                var shipId = npcs.NpcId;
                if (!GameStorageManager.Instance.Ships.ContainsKey(shipId))
                {
                    Out.QuickLog("NPC of type " + shipId + " doesn't exist in Ships storage");
                    throw new Exception("NPC of that type does not exist in Ships dictionary");
                }
                
                var ship = GameStorageManager.Instance.Ships[shipId];
                CreateNpc(map, ship);
            }
        }

        public void CreateNpc(Spacemap map, Ship ship)
        {
            //todo: create
        }

        public void RemoveNpc(Npc npc)
        {
            //todo: removing it without destroy or any alert. simply gone...
        }
        
        public void CreateGameObject(Spacemap map, GameObject gameObject) {}
        
        public void RemoveGameObject(GameObject gameObject) {}
    }
}