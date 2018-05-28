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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.objects.jumpgates;
using NettyBaseReloaded.Game.objects.world.map.objects.stations;
using NettyBaseReloaded.Game.objects.world.map.ores;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.map.zones.pallazones;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;
using Types = NettyBaseReloaded.Game.objects.world.map.collectables.Types;

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
        public Vector[] Limits { get; private set; }
        public int Level { get; }
        public bool Disabled { get; set; }
        public bool RangeDisabled { get; set; }

        #region Base Storages [DB]
        private List<PortalBase> PortalBase { get; set; }
        private List<BaseNpc> Npcs;
        #endregion

        public Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();

        public ConcurrentDictionary<int, Object> Objects = new ConcurrentDictionary<int, Object>();

        public ConcurrentDictionary<string, Object> HashedObjects = new ConcurrentDictionary<string, Object>();

        public Dictionary<string, POI> POIs = new Dictionary<string, POI>();

        //Used to store all the entities of the map
        public ConcurrentDictionary<int, Character> Entities = new ConcurrentDictionary<int, Character>();

        public Dictionary<int, Spacemap> VirtualWorlds = new Dictionary<int, Spacemap>()
        {
            {0, null} // Adding virtual worlds after id = 0 since it might cause bugs
        };

        public Spacemap(int id, string name, Faction faction, bool pvp, bool starter, int level, List<BaseNpc> npcs, List<PortalBase> portals)
        {
            Id = id;
            Name = name;
            Faction = faction;
            Pvp = isPvp();
            Starter = starter;
            ParseLimits();
            Level = level;
            PortalBase = portals;
            Npcs = npcs;

            Global.TickManager.Add(this);
        }

        private void ParseLimits()
        {
            Limits = new Vector[]{null, null};
            Limits[0] = new Vector(0, 0);
            if (Id == 16 || Id == 29)
                Limits[1] = new Vector(41800, 26000);
            else Limits[1] = new Vector(20800, 12800);
        }

        private bool isPvp()
        {
            return Id == 16 || Id == 15 || Id == 14 || Id == 13;
        }

        public void Tick()
        {
            ObjectsTicker();
            //ZoneTicker();
            PlayerTicker();
            //NpcTicker();
        }

        private DateTime LastTimeTicketObjects = new DateTime();
        public void ObjectsTicker()
        {
            if (LastTimeTicketObjects.AddSeconds(2) > DateTime.Now) return;
            foreach (var obj in Objects)
            {
                obj.Value?.Tick();
            }
            LastTimeTicketObjects = DateTime.Now;
        }

        public void ZoneTicker()
        {
            
        }

        public bool InNonPlayArea(Vector position)
        {
            if (Id == 16 || Id == 29)
            {
                return position.X < 0 || position.X > 41800 || position.Y < 0 || position.Y > 26000;
            }
            return position.X < 0 || position.X > 20800 || position.Y < 0 || position.Y > 12800;
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
                    if (player.Spacemap != this)
                    {
                        RemoveEntity(player);
                        continue;
                    }
                    if (player.Storage.LoadedObjects.Count != Objects.Count)
                    {
                        var dicOne = player.Storage.LoadedObjects.ToList();
                        var dicTwo = Objects;
                        var diff = dicOne.Except(dicTwo).Concat(dicTwo.Except(dicOne));

                        foreach (var differance in diff)
                        {
                            if (Objects.ContainsKey(differance.Key))
                                player.LoadObject(differance.Value);
                            else if (player.Storage.LoadedObjects.ContainsKey(differance.Key))
                                player.UnloadObject(differance.Value);
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
        #region Thread Safe Adds / Removes

        public event EventHandler<CharacterArgs> EntityAdded;
        public bool AddEntity(Character character)
        {
            var success = Entities.TryAdd(character.Id, character);
            if (success) EntityAdded?.Invoke(this, new CharacterArgs(character));
            return success;
        }

        public event EventHandler<CharacterArgs> EntityRemoved;
        public bool RemoveEntity(Character character)
        {
            var success = Entities.TryRemove(character.Id, out character);
            if (success) EntityRemoved?.Invoke(this, new CharacterArgs(character));
            return success;
        }

        public bool AddObject(Object obj)
        {
            return Objects.TryAdd(obj.Id, obj);
        }

        public bool AddObject(int id)
        {
            return Objects.TryAdd(id, null);
        }

        public bool RemoveObject(Object obj)
        {
            obj.Position = null;
            return Objects.TryRemove(obj.Id, out obj);
        }

        #endregion

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
            using (var enumerator = Objects.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return 0;

                var nextKeyInSequence = enumerator.Current.Key + 1;

                if (nextKeyInSequence < 1)
                    throw new InvalidOperationException("The dictionary contains keys less than 0");

                if (nextKeyInSequence != 1)
                    return 0;

                while (enumerator.MoveNext())
                {
                    var key = enumerator.Current.Key;
                    if (key > nextKeyInSequence)
                        return nextKeyInSequence;

                    ++nextKeyInSequence;
                }

                return nextKeyInSequence;
            }
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

            if (Properties.Server.DEBUG)
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
                    position = Vector.Random(this, zone.TopLeft.X, zone.BottomRight.X, zone.TopLeft.Y, zone.BottomRight.Y);
                else position = Vector.Random(this, 1000, 20000, 1000, 12000);
                CreateNpc(new Npc(id, ship.Name,
                    new Hangar(ship, new List<Drone>(), position, this, ship.Health, ship.Nanohull,
                        new Dictionary<string, Item>()),
                    0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.Shield,
                    ship.Damage));
            }
        }

        public void CreateNpc(Ship ship)
        {
            var id = GetNextAvailableId();
            var position = Vector.Random(this, 1000, 20000, 1000, 12000);
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(ship, new List<Drone>(), position, this, ship.Health, ship.Nanohull,
                    new Dictionary<string, Item>()),
                0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.Shield,
                ship.Damage));
        }

        public void CreateNpc(Ship ship, AILevels ai, bool respawning, int respawnTime, Vector pos = null, int vwId = 0)
        {
            var id = GetNextAvailableId();
            ship.AI = (int)ai;
            if (pos == null)
                pos = Vector.Random(this, 1000, 20000, 1000, 12000);
            else pos = Vector.GetPosOnCircle(pos, 100);
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(ship, new List<Drone>(), pos, this, ship.Health, ship.Nanohull,
                    new Dictionary<string, Item>()),
                0, pos, this, ship.Health, ship.Nanohull, ship.Reward, ship.Shield,
                ship.Damage, respawnTime, respawning){VirtualWorldId = vwId});
        }

        public int CreateNpc(Ship ship, AILevels ai, Npc motherShip)
        {
            var id = GetNextAvailableId();
            ship.AI = (int) ai;
            var position = motherShip.Position;
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(ship, new List<Drone>(), position, this, ship.Health, ship.Nanohull,
                    new Dictionary<string, Item>()),
                0, position, this, ship.Health, ship.Nanohull, ship.Reward, ship.Shield,
                ship.Damage, 0, false, motherShip));
            return id;
        }

        public void CreateNpc(Npc npc)
        {
            var id = npc.Id;
            if (Entities.ContainsKey(npc.Id))
            {
                if (Properties.Server.DEBUG)
                    Console.WriteLine("Failed adding NPC [ID: " + id + "]");
                return;
            }

            AddEntity(npc);

            if (!Global.TickManager.Exists(npc))
                Global.TickManager.Add(npc);
            npc.Controller = new NpcController(npc);
            npc.Controller.Initiate();
            World.Log.Write("Created NPC[" + npc.Id + ", " + npc.Hangar.Ship.ToStringLoot() + "] on mapId " + Id);
        }

        #endregion

        #region Zones

        public void CreateDemiZone(Vector topLeft, Vector botRight)
        {
            var id = GetNextZoneId();
            if (!Pvp)
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

        public void CreatePortal(int map, int x, int y, int newX, int newY, int vwId = 0)
        {
            var id = GetNextObjectId();
            AddObject(new Jumpgate(id, 0, new Vector(x, y), this, new Vector(newX, newY), map, true, 0, 0, 1));

            var zoneId = GetNextZoneId();
            if (!Pvp)
                Zones.Add(zoneId, new DemiZone(zoneId, new Vector(x - 500, y + 500), new Vector(x + 500, y - 500)));
            World.Log.Write("Created Portal on mapId " + Id);
        }

        public void CreateLoW(Vector pos)
        {
            var id = GetNextObjectId();
            AddObject(new LowPortal(id, pos, this, 0));

            var zoneId = GetNextZoneId();
                Zones.Add(zoneId, new DemiZone(zoneId, new Vector(pos.X - 500, pos.Y + 500), new Vector(pos.X + 500, pos.Y - 500)));
        }

        public void CreateHiddenPortal(int map, int x, int y, int newX, int newY, int vwId = 0)
        {
            var id = GetNextObjectId();
            AddObject(new Jumpgate(id, 0, new Vector(x, y), this, new Vector(newX, newY), map, false, 0, 0, 0));
            World.Log.Write("Created Portal on mapId " + Id);
        }


        public void CreateStation(Faction faction, Vector position)
        {
            var assignedStationIds = new List<int>();
            for (int i = 0; i <= 4; i++)
            {
                var id = GetNextObjectId();
                assignedStationIds.Add(id);
                AddObject(id);
            }

            var hqModule = new StationModule(assignedStationIds[0], position, this, AssetTypes.HQ);
            var hangarModule = new StationModule(assignedStationIds[1], Vector.FromVector(position, 800, 0), this, AssetTypes.HANGAR);
            var resourceModule = new StationModule(assignedStationIds[2], Vector.FromVector(position, 0, 800), this, AssetTypes.ORE_TRADE);
            var repairModule = new StationModule(assignedStationIds[3], Vector.FromVector(position, 0, -800), this, AssetTypes.REPAIR_DOCK);

            Objects[assignedStationIds[0]] = new Station(assignedStationIds[0], new List<StationModule>{hqModule, hangarModule, resourceModule, repairModule}, faction, position, this);
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
            AddObject(new PirateStation(id, vector, this));
            World.Log.Write("Created Pirate Station on mapId " + Id);
        }

        public void CreateHealthStation(Vector vector)
        {
            var id = GetNextObjectId();
            AddObject(new HealthStation(id, vector, this));
            World.Log.Write("Created Health Station on mapId " + Id);
        }

        public void CreateRelayStation(Vector vector)
        {
            var id = GetNextObjectId();
            AddObject(new RelayStation(id, vector, this));
            World.Log.Write("Created Relay Station on mapId " + Id);
        }

        public void CreateReadyRelayStation(Vector vector)
        {
            var id = GetNextObjectId();
            AddObject(new ReadyRelayStation(id, vector, this));
            World.Log.Write("Created Relay Station on mapId " + Id);
        }

        public void CreateQuestGiver(Faction faction, Vector pos)
        {
            var id = GetNextObjectId();
            var questGiverId = World.StorageManager.QuestGivers.Count;
            var questGiver = new QuestGiver(id, questGiverId, faction, pos, this);
            World.StorageManager.QuestGivers.Add(questGiverId, questGiver);
            AddObject(questGiver);
            World.Log.Write("Created Quest Giver on mapId " + Id);
        }

        public void CreateAsteroid(string name, Vector pos)
        {
            var id = GetNextObjectId();
            var bStation = World.StorageManager.ClanBattleStations.Count;
            World.StorageManager.ClanBattleStations.Add(bStation, null);
            AddObject(new Asteroid(id, bStation, name, pos, this));
            World.Log.Write("Created Asteroid on mapId " + Id);
        }

        public void CreateAdvertisementBanner(short advertiser, Vector pos)
        {
            var id = GetNextObjectId();
            AddObject(new Billboard(id, pos, this, advertiser));
        }

        #endregion

        #region Collectables

        public void CreateOre(OreTypes type, Vector pos, int[] limits)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new PalladiumOre(id, hash, type, pos, this, limits);
            HashedObjects[hash] = box;
            if(AddObject(box))
                World.Log.Write("Created Ore["+type+"] on mapId " + Id);
        }

        public void CreateBox(Types type, Vector pos, int[] limits)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new BonusBox(id, hash, pos, this, limits,true);
            HashedObjects[hash] = box;
            if (AddObject(box))
                World.Log.Write("Created Box[" + type + "] on mapId " + Id);
        }

        public void CreateShipLoot(Vector position, DropableRewards content, Character killer)
        {
            //Console.WriteLine($"Created ship loot (content)->{content}");
            if (content != null)
            {
                var id = GetNextObjectId();
                var hash = HashedObjects.Keys.ToList()[id];
                var box = new CargoLoot(id, hash, position, content, killer);
                HashedObjects[hash] = box;
                if (AddObject(box))
                    World.Log.Write($"Created cargo loot ({position.X},{position.Y}) on mapId " + Id);
            }
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

        public void CreatePalladiumField()
        {
            if (Id != 16)
                return;

            var zoneId = GetNextZoneId();
            PalladiumZone zone = new PalladiumZone1(zoneId);
            Zones.Add(zoneId, zone);
            CreatePOI(new POI("smoke_01", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE, new List<Vector> { new Vector(0, 16200), new Vector(5000, 25500), new Vector(0, 25500), new Vector(5000, 16200) }));

            zoneId = GetNextZoneId();
            zone = new PalladiumZone2(zoneId);
            Zones.Add(zoneId, zone);
            CreatePOI(new POI("smoke_02", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE, new List<Vector> { new Vector(4900, 17700), new Vector(5800, 25400), new Vector(4900, 25400), new Vector(5800, 17700) }));

            zoneId = GetNextZoneId();
            zone = new PalladiumZone3(zoneId);
            Zones.Add(zoneId, zone);
            CreatePOI(new POI("smoke_03", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE, new List<Vector> { new Vector(5700, 18800), new Vector(7700, 25500), new Vector(5700, 25500), new Vector(7700, 18800) }));

            zoneId = GetNextZoneId();
            zone = new PalladiumZone4(zoneId);
            Zones.Add(zoneId, zone);
            CreatePOI(new POI("smoke_04", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE, new List<Vector> { new Vector(7600, 21100), new Vector(24700, 25500), new Vector(7600, 25500), new Vector(24700, 21100) }));

            zoneId = GetNextZoneId();
            zone = new PalladiumZone5(zoneId);
            Zones.Add(zoneId, zone);
            CreatePOI(new POI("smoke_05", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE, new List<Vector> { new Vector(14600, 20600), new Vector(24700, 21100), new Vector(14600, 21100), new Vector(24700, 20600) }));

            zoneId = GetNextZoneId();
            zone = new PalladiumZone6(zoneId);
            Zones.Add(zoneId, zone);
            CreatePOI(new POI("smoke_06", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE, new List<Vector> { new Vector(7600, 20700), new Vector(12300, 21500), new Vector(7600, 21500), new Vector(12300, 20700) }));

            foreach (var _zone in Zones.Where(x => x.Value is PalladiumZone))
            {
                for (var i = 0; i < 60; i++)
                    CreateOre(OreTypes.PALLADIUM, Vector.Random(this, _zone.Value.TopLeft.X, _zone.Value.BottomRight.X, _zone.Value.TopLeft.Y, _zone.Value.BottomRight.Y), new [] { _zone.Value.TopLeft.X, _zone.Value.BottomRight.X, _zone.Value.TopLeft.Y, _zone.Value.BottomRight.Y });
            }
        }

        #endregion

        #region Virtual Worlds

        public int AddVirtualWorld(out Spacemap map)
        {
            map = new Spacemap(Id, Name, Faction, Pvp, Starter, Level, new List<BaseNpc>(), new List<PortalBase>());
            var id = VirtualWorlds.FirstOrDefault(x => x.Value == null && x.Key != 0).Key;
            VirtualWorlds.Add(id, map);
            return id;
        }

        #endregion

        public static void Duplicate(Spacemap map, out Spacemap duped)
        {
            duped = new Spacemap(map.Id, map.Name, map.Faction, map.Pvp, map.Starter, map.Level, map.Npcs,
                map.PortalBase)
            {
                Disabled = false,
                HashedObjects = map.HashedObjects,
                POIs = map.POIs,
                Limits = map.Limits,
                RangeDisabled = map.RangeDisabled,
                Objects = map.Objects
            };
        }
    }
}
