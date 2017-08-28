using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world
{
    class Spacemap : ITick
    {
        // TODO: Define all the zones in a class
        // DemiZone / PoiZone

        /**********
         * BASICS *
         **********/
        public int Id { get; }
        public string Name { get; }
        public Faction Faction { get; }
        public bool Pvp { get; }
        public bool Starter { get; }
        public string Limits { get; }
        public int Level { get; }

        #region Base Storages [DB]
        private List<PortalBase> PortalBase { get; set; }
        private List<BaseNpc> Npcs;
        #endregion

        public Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();

        public Dictionary<int, Object> Objects = new Dictionary<int, Object>();

        public Dictionary<string, Collectable> Collectables = new Dictionary<string, Collectable>();

        public Dictionary<string, POI> POIs = new Dictionary<string, POI>();

        //Used to store all the entities of the map
        public Dictionary<int, Character> Entities = new Dictionary<int, Character>();

        public Spacemap(int id, string name, Faction faction, bool pvp, bool starter, int level, List<BaseNpc> npcs, List<PortalBase> portals)
        {
            Id = id;
            Name = name;
            Faction = faction;
            Pvp = pvp;
            Starter = starter;
            Limits = "208x128";
            Level = level;
            PortalBase = portals;
            Npcs = npcs;

            Global.TickManager.Tickables.Add(this);
        }

        public void Tick()
        {
            ObjectsTicker();
            ZoneTicker();
            PlayerTicker();
        }

        public void ObjectsTicker()
        {
            // TODO
        }

        public void ZoneTicker()
        {
            // TODO
        }

        private DateTime LastTimeTickedPlayers = new DateTime();
        public void PlayerTicker()
        {
            if (LastTimeTickedPlayers.AddSeconds(2) > DateTime.Now) return;

            foreach (var entity in Entities)
            {
                var player = entity.Value as Player;
                if (player != null)
                {
                    if (player.EntitiesStorage.LoadedObjects.Count != Objects.Count)
                    {
                        var dicOne = player.EntitiesStorage.LoadedObjects;
                        var dicTwo = Objects;
                        var diff = dicOne.Except(dicTwo).Concat(dicTwo.Except(dicOne));

                        foreach (var differance in diff)
                        {
                            player.LoadObject(differance.Value);
                        }
                    }

                    if (player.EntitiesStorage.LoadedPOI.Count != POIs.Count)
                    {
                        var dicOne = player.EntitiesStorage.LoadedPOI;
                        var dicTwo = POIs;
                        var diff = dicOne.Except(dicTwo).Concat(dicTwo.Except(dicOne));

                        foreach (var differance in diff)
                        {
                            player.LoadPOI(differance.Value);
                        }
                    }
                }
            }
            LastTimeTickedPlayers = DateTime.Now;
        }

        #region IDs
        public int GetNextAvailableId()
        {
            var lastId = 1000000;
            foreach (var entity in Entities.Keys.Where(x => x > 1000000))
            {
                if (entity == lastId + 1)
                    lastId++;
                else return lastId + 1;
            }
            return lastId + 1;
        }

        public int GetNextObjectId()
        {
            var lastId = 0;
            foreach (var obj in Objects.Keys)
            {
                if (obj == lastId + 1)
                    lastId++;
                else return lastId + 1;
            }
            return lastId + 1;
        }

        public int GetNextZoneId()
        {
            var lastId = 0;
            foreach (var obj in Zones.Keys)
            {
                if (obj == lastId + 1)
                    lastId++;
                else return lastId + 1;
            }
            return lastId + 1;
        }


        public string GetRandomHash()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region NPCs

        public void SpawnNpcs()
        {
            if (Npcs == null) return;

            int npcSpawned = 0;
            foreach (var entry in Npcs)
            {
                for (int i = 0; i <= entry.Count; i++)
                {
                    CreateNpcs(entry);
                    npcSpawned++;
                }
            }

            Out.WriteLine("Successfully spawned " + npcSpawned + " npcs on spacemap " + Name);
        }

        public void CreateNpcs(BaseNpc baseNpc, Zone zone = null)
        {
            for (int i = 0; i <= baseNpc.Count; i++)
            {
                var id = GetNextAvailableId();
                var ship = World.StorageManager.Ships[baseNpc.NpcId];
                var position = new Vector(0,0);
                if (zone != null)
                    position = Vector.Random(zone.TopLeft.X, zone.BottomRight.X, zone.TopLeft.Y, zone.BottomRight.Y);
                else position = Vector.Random(1000, 20000, 1000, 12000);
                CreateNpc(new Npc(id, ship.Name,
                    new Hangar(ship, new List<Drone>(), position, this, ship.Health, ship.Nanohull,
                        new Dictionary<string, Item>()),
                    0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.DropableRewards, ship.Shield,
                    ship.Damage));
            }
        }

        public void CreateNpc(Ship ship)
        {
            var id = GetNextAvailableId();
            var position = Vector.Random(1000, 20000, 1000, 12000);
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(ship, new List<Drone>(), position, this, ship.Health, ship.Nanohull,
                    new Dictionary<string, Item>()),
                0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.DropableRewards, ship.Shield,
                ship.Damage));

        }

        public void CreateNpc(Npc npc)
        {
            var id = npc.Id;
            if (Entities.ContainsKey(npc.Id))
            {
                Console.WriteLine("Failed adding NPC [ID: " + id + "]");
                return;
            }

            Entities.Add(id, npc);

            npc.Controller = new NpcController(npc);
            npc.Controller.Start((AILevels) npc.Hangar.Ship.AI);
        }

        public void InitiateAI()
        {
            foreach (var entity in Entities)
            {
                var npc = entity.Value as Npc;
                npc?.Controller.Start((AILevels) npc.Hangar.Ship.AI);
            }
        }
        #endregion

        #region Zones

        public void CreateDemiZone(Vector topLeft, Vector botRight)
        {
            var id = GetNextZoneId();
            Zones.Add(id, new DemiZone(id, topLeft, botRight));
            Console.WriteLine("Zone added [ID: {0}, topLeft: {1} botRight: {2}]", id, topLeft.ToString(), botRight.ToString());
        }

        public void LoadZones()
        {
            foreach (var zone in Zones.Values)
            {
                if (zone is map.zones.ResourceZone)
                {
                    ResourceZone(zone as ResourceZone);
                }
            }
        }

        private void ResourceZone(ResourceZone zone)
        {
            var density = zone.Density;
            var resType = zone.Type;

            for (int i = 0; i <= (1000 * density); i++)
            {
                switch (resType)
                {
                    case Types.BONUS_BOX:
                        CreateBox();
                        break;
                }
            }
        }

        #endregion

        #region Objects

        public void LoadObjects()
        {
            if (PortalBase == null || PortalBase.Count == 0)
            {
                return;
            }
            
            foreach (var portal in PortalBase)
            {
                CreatePortal(portal.Map, portal.x, portal.y, portal.newX, portal.newY);
            }            
        }

        public void CreatePortal(int map, int x, int y, int newX, int newY)
        {
            var id = GetNextObjectId();
            Objects.Add(id, new Jumpgate(id, 0, new Vector(x, y), new Vector(newX, newY), map, true, 0, 0, 1));

            var zoneId = GetNextZoneId();
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(x - 500, y + 500), new Vector(x + 500, y - 500)));
        }

        public void CreateStation(Faction faction, Vector position)
        {
            var assignedStationIds = new List<int>();
            for (int i = 0; i <= 4; i++)
            {
                var id = GetNextObjectId();
                assignedStationIds.Add(id);
                Objects.Add(id, null);
            }

            Objects[assignedStationIds[0]] = new Station(assignedStationIds[0], assignedStationIds, faction, position);

            var zoneId = GetNextZoneId();
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(position.X - 1000, position.Y + 1000), new Vector(position.X + 1000, position.Y - 1000)));
        }

        public void CreatePirateStation(Vector vector)
        {
            var id = GetNextObjectId();
            Objects.Add(id, new PirateStation(id, vector));
        }

        #endregion

        #region Collectables

        public void CreateOre()
        {
            throw new NotImplementedException();
        }

        public void CreateBox()
        {
            throw new NotImplementedException();
        }

        public void CreateShipLoot(Vector position, DropableRewards content)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region POI

        public POI GetPOI(string id)
        {
            if (POIs[id] != null)
                return POIs[id];
            return null;
        }

        public void CreatePOI(POI poi)
        {
            if (POIs.ContainsKey(poi.Id)) return;
            POIs.Add(poi.Id, poi);
        }

        #endregion
    }

}
