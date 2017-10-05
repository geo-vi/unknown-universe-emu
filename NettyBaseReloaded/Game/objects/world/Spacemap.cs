using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.equipment;
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
        public bool Disabled { get; set; }

        #region Base Storages [DB]
        private List<PortalBase> PortalBase { get; set; }
        private List<BaseNpc> Npcs;
        #endregion

        public Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();

        public Dictionary<int, Object> Objects = new Dictionary<int, Object>();

        public Dictionary<string, Object> HashedObjects = new Dictionary<string, Object>();

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

            Global.TickManager.Add(this);
        }

        public void Tick()
        {
            ObjectsTicker();
            ZoneTicker();
            PlayerTicker();
            //NpcTicker();
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

            foreach (var entity in Entities.ToList())
            {
                var player = entity.Value as Player;
                if (player != null)
                {
                    if (player.Storage.LoadedObjects.Count != Objects.Count)
                    {
                        var dicOne = player.Storage.LoadedObjects;
                        var dicTwo = Objects;
                        var diff = dicOne.Except(dicTwo).Concat(dicTwo.Except(dicOne));

                        foreach (var differance in diff.ToList())
                        {
                            if (Objects.ContainsKey(differance.Key))
                                player.LoadObject(differance.Value);
                            else
                                player.Storage.LoadedObjects.Remove(differance.Key);
                        }
                    }

                    if (player.Storage.LoadedPOI.Count != POIs.Count)
                    {
                        var dicOne = player.Storage.LoadedPOI;
                        var dicTwo = POIs;
                        var diff = dicOne.Except(dicTwo).Concat(dicTwo.Except(dicOne));

                        foreach (var differance in diff)
                        {
                            player.Storage.LoadPOI(differance.Value);
                        }
                    }
                }
            }
            LastTimeTickedPlayers = DateTime.Now;
        }

        public void NpcTicker()
        {
            if (Entities.Count > 0)
            {
                var entries = Entities.ToList().Where(x => x.Value is Npc);
                foreach (var entry in entries)
                {
                    ((Npc)entry.Value).Tick();
                }
            }
        }

        #region IDs
        public int GetNextAvailableId()
        {
            var lastId = 1000000;
            foreach (var entity in Entities.Keys.ToList().Where(x => x > 1000000))
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

        #endregion

        #region NPCs

        public void SpawnNpcs()
        {
            if (Npcs == null) return;

            int npcSpawned = 0;
            foreach (var entry in Npcs)
            {
                CreateNpcs(entry);
                npcSpawned += entry.Count;
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
                    0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.CargoDrop, ship.Shield,
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
                0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.CargoDrop, ship.Shield,
                ship.Damage));
        }

        public void CreateNpc(Ship ship, AILevels ai, int respawnTime)
        {
            var id = GetNextAvailableId();
            ship.AI = (int)ai;
            var position = Vector.Random(1000, 20000, 1000, 12000);
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(ship, new List<Drone>(), position, this, ship.Health, ship.Nanohull,
                    new Dictionary<string, Item>()),
                0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.CargoDrop, ship.Shield,
                ship.Damage, respawnTime));
        }

        public int CreateNpc(Ship ship, AILevels ai, Npc motherShip)
        {
            var id = GetNextAvailableId();
            ship.AI = (int) ai;
            var position = motherShip.Position;
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(ship, new List<Drone>(), position, this, ship.Health, ship.Nanohull,
                    new Dictionary<string, Item>()),
                0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.CargoDrop, ship.Shield,
                ship.Damage, 0, motherShip));
            return id;
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
            if (Id == 12 || Id == 11 || Id == 10 || Id == 9)
                npc.Controller.Initiate();
            World.Log.Write("Created NPC[" + npc.Id + ", " + npc.Hangar.Ship.ToStringLoot() + "] on mapId " + Id);
        }

        #endregion

        #region Zones

        public void CreateDemiZone(Vector topLeft, Vector botRight)
        {
            var id = GetNextZoneId();
            Zones.Add(id, new DemiZone(id, topLeft, botRight));
            Console.WriteLine("Zone added [ID: {0}, topLeft: {1} botRight: {2}]", id, topLeft.ToString(), botRight.ToString());
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
            World.Log.Write("Loaded objects on mapId " + Id);
        }

        public void CreatePortal(int map, int x, int y, int newX, int newY)
        {
            var id = GetNextObjectId();
            Objects.Add(id, new Jumpgate(id, 0, new Vector(x, y), new Vector(newX, newY), map, true, 0, 0, 1));

            var zoneId = GetNextZoneId();
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(x - 500, y + 500), new Vector(x + 500, y - 500)));
            World.Log.Write("Created Portal on mapId " + Id);
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

            var hqModule = new StationModule(assignedStationIds[0], position, AssetTypeModule.HQ);
            var hangarModule = new StationModule(assignedStationIds[1], Vector.FromVector(position, 800, 0), AssetTypeModule.HANGAR);
            var resourceModule = new StationModule(assignedStationIds[2], Vector.FromVector(position, 0, 800), AssetTypeModule.ORE_TRADE);
            var repairModule = new StationModule(assignedStationIds[3], Vector.FromVector(position, 0, -800), AssetTypeModule.REPAIR_DOCK);

            Objects[assignedStationIds[0]] = new Station(assignedStationIds[0], new List<StationModule>{hqModule, hangarModule, resourceModule, repairModule}, faction, position);
            Objects[assignedStationIds[1]] = hangarModule;
            Objects[assignedStationIds[2]] = resourceModule;
            Objects[assignedStationIds[3]] = repairModule;

            var zoneId = GetNextZoneId();
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(position.X - 1000, position.Y + 1000), new Vector(position.X + 1000, position.Y - 1000)));
            World.Log.Write("Created Station on mapId " + Id);
        }

        public void CreatePirateStation(Vector vector)
        {
            var id = GetNextObjectId();
            Objects.Add(id, new PirateStation(id, vector));
            World.Log.Write("Created Pirate Station on mapId " + Id);
        }

        public void CreateQuestGiver(Faction faction, Vector pos)
        {
            var id = GetNextObjectId();
            Objects.Add(id, new QuestGiver(id, faction, pos));
            World.Log.Write("Created Quest Giver on mapId " + Id);
        }

        public void CreateAsteroid(string name, Vector pos)
        {
            var id = GetNextObjectId();
            Objects.Add(id, new Asteroid(id, name, pos));
            World.Log.Write("Created Asteroid on mapId " + Id);
        }

        #endregion

        #region Collectables

        public void CreateOre(OreTypes type, Vector pos)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new Ore(id, hash, type, pos);
            HashedObjects[hash] = box;
            Objects.Add(id, box);
            World.Log.Write("Created Ore["+type+"] on mapId " + Id);
        }

        public void CreateBox(Types type, Vector pos)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new BonusBox(id, hash, pos);
            HashedObjects[hash] = box;
            Objects.Add(id, box);
            World.Log.Write("Created Box[" + type + "] on mapId " + Id);
        }

        public void CreateShipLoot(Vector position, CargoDrop content)
        {
            World.Log.Write("NotImplentedException");
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
