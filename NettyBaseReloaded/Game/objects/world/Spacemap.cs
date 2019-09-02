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
using NettyBaseReloaded.Game.objects.world.npcs;
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
        private int TickId { get; set; }
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
            Pvp = IsPvp();
            Starter = starter;
            ParseLimits();
            Level = level;
            PortalBase = portals;
            Npcs = npcs;

            var tickId = -1;
            Global.TickManager.Add(this, out tickId);
            TickId = tickId;
        }

        private void ParseLimits()
        {
            Limits = new Vector[]{null, null};
            Limits[0] = new Vector(0, 0);
            if (Id == 16 || Id == 29 || Id == 91 || Id == 93)
                Limits[1] = new Vector(41800, 26000);
            else Limits[1] = new Vector(20800, 12800);
        }

        private bool IsPvp()
        {
            return Id == 16 || Id == 15 || Id == 14 || Id == 13 || Id >= 29;
        }

        public int GetId()
        {
            return TickId;
        }

        public void Tick()
        {
            ObjectsTicker();
        }

        private DateTime LastTimeTicketObjects = new DateTime();
        public void ObjectsTicker()
        {
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
            return position.X < Limits[0].X || position.X > Limits[1].X || position.Y < Limits[0].Y || position.Y > Limits[1].Y;
        }
        
        #region Thread Safe Adds / Removes

        public bool AddEntity(Character character)
        {
            var success = Entities.TryAdd(character.Id, character);
            //if (success) EntityAdded?.Invoke(this, new CharacterArgs(character));
            return success;
        }

        public bool RemoveEntity(Character character)
        {
            Character output;
            var success = Entities.TryRemove(character.Id, out output);
            //if (success) EntityRemoved?.Invoke(this, new CharacterArgs(character));
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

        public event EventHandler<Object> RemovedObject;
        public bool RemoveObject(Object obj)
        {
            if (obj == null) return false;
            RemovedObject?.Invoke(this, obj);
            obj.Position = null;
            return Objects.TryRemove(obj.Id, out obj);
        }

        #endregion

        #region IDs
        public int GetNextAvailableId(int startId = 1000000)
        {
            var i = startId;
            while (true)
            {
                if (Entities.ContainsKey(i))
                    i++;
                else return i;
            }
        }

        public int GetNextObjectId(int start = 0)
        {
            var i = start;
            while (true)
            {
                if (Objects.ContainsKey(i))
                    i++;
                else return i;
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

            Debug.WriteLine("Successfully spawned " + npcSpawned + " npcs on spacemap " + Name);
        }

        public void CreateNpcs(BaseNpc baseNpc, Zone zone = null)
        {
            for (int i = 0; i <= baseNpc.Count; i++)
            {
                var id = GetNextAvailableId();
                var ship = World.StorageManager.Ships[baseNpc.NpcId];
                var position = new Vector(0,0);
                if (zone != null)
                    position = Vector.Random(this, zone.TopLeft, zone.BottomRight);
                else position = Vector.Random(this, new Vector(1000, 1000), new Vector(20000, 12000));
                Npc npc = null;
                if (ship.Id == 112)
                {
                    npc = new Barracuda(id, ship.Name,
                        new Hangar(0, ship, new Dictionary<int, Drone>(), position, this, ship.Health, ship.Nanohull),
                        0, position, this, ship.Health, 0, ship.Shield,
                        ship.Shield, ship.Health, ship.Damage, ship.Reward, 5);
                }
                else
                {
                    npc = new Npc(id, ship.Name,
                        new Hangar(0, ship, new Dictionary<int, Drone>(), position, this, ship.Health, ship.Nanohull),
                        0, position, this, ship.Health, 0, ship.Shield,
                        ship.Shield, ship.Health, ship.Damage, ship.Reward, 5);
                }
                CreateNpc(npc);
            }
        }

        public void CreateNpc(Ship ship)
        {
            var id = GetNextAvailableId();
            var position = Vector.Random(this, new Vector(1000, 1000), new Vector(20000, 12000));
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(0, ship, new Dictionary<int, Drone>(), position, this, ship.Health, ship.Nanohull),
                0, position, this, ship.Health, 0, ship.Shield,
                ship.Shield, ship.Health, ship.Damage, ship.Reward, 5));
        }

        public Npc CreateNpc(Ship ship, AILevels ai, bool respawning, int respawnTime, Vector pos = null, int vwId = 0, string namePrefix = "", Reward reward = null)
        {
            var id = GetNextAvailableId();
            ship.AI = (int)ai;
            if (pos == null)
                pos = Vector.Random(this, new Vector(1000, 1000), new Vector(20000, 12000));
            else pos = Vector.GetPosOnCircle(pos, 100);

            var name = ship.Name;
            if (namePrefix != "")
            {
                name += " " + namePrefix;
            }

            if (reward == null)
                reward = ship.Reward;

            var npc = new Npc(id, name,
                    new Hangar(0, ship, new Dictionary<int, Drone>(), pos, this, ship.Health, ship.Nanohull),
                    0, pos, this, ship.Health, 0, ship.Shield,
                    ship.Shield, ship.Health, ship.Damage, reward, respawnTime, respawning)
                {VirtualWorldId = vwId};
            CreateNpc(npc);
            return npc;
        }

        public int CreateNpc(Ship ship, AILevels ai, Npc motherShip)
        {
            var id = GetNextAvailableId();
            ship.AI = (int) ai;
            var position = motherShip.Position;
            CreateNpc(new Npc(id, ship.Name,
                new Hangar(0, ship, new Dictionary<int, Drone>(), position, this, ship.Health, ship.Nanohull),
                0, position, this, ship.Health, 0, ship.Shield,
                ship.Shield, ship.Health, ship.Damage, ship.Reward, 0, false, motherShip));
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

            var tickId = -1;
            Global.TickManager.Add(npc, out tickId);
            npc.SetTickId(tickId);
            npc.RocketLauncher = npc.Hangar.Ship.GetRocketLauncher(npc);
            npc.Controller = new NpcController(npc);
            npc.Controller.Initiate();
            Out.WriteLog("Created NPC [" + npc.Id + ", " + npc.Hangar.Ship.ToStringLoot() + "] on mapId " + Id);
        }

        public void CreateCubikon(Vector position)
        {
            var id = GetNextAvailableId();
            var tickId = -1;
            var ship = World.StorageManager.Ships[80];
            var npc = new Cubikon(id, ship.Name,
                new Hangar(0, ship, new Dictionary<int, Drone>(), position, this, ship.Health, ship.Nanohull),
                0, position, this, ship.Health, 0, ship.Shield,
                ship.Shield, ship.Health, ship.Damage, ship.Reward, 25);

            if (Entities.ContainsKey(npc.Id))
            {
                if (Properties.Server.DEBUG)
                    Console.WriteLine(id + "#Failed adding NPC");
                return;
            }

            AddEntity(npc);

            if (!Global.TickManager.Exists(npc))
            {
                Global.TickManager.Add(npc, out tickId);
                npc.SetTickId(tickId);
            }
            
            npc.Controller = new NpcController(npc) { CustomSetAI = AILevels.MOTHERSHIP };
            npc.Controller.Initiate();
        }

        public void CreateSpaceball(int designId, Vector position)
        {
            var id = GetNextAvailableId();
            var ship = World.StorageManager.Ships[designId];
            var npc = new Spaceball(id, ship.Name,
                new Hangar(0, ship, new Dictionary<int, Drone>(), position, this, ship.Health, ship.Nanohull),
                0, position, this, ship.Health, 0, ship.Shield,
                ship.Shield, ship.Health, ship.Damage, ship.Reward, 90);

            if (Entities.ContainsKey(npc.Id))
            {
                if (Properties.Server.DEBUG)
                    Console.WriteLine(id + "#Failed adding NPC");
                return;
            }

            AddEntity(npc);

            if (!Global.TickManager.Exists(npc))
            {
                var tickId = -1;
                Global.TickManager.Add(npc, out tickId);
                npc.SetTickId(tickId);
            }

            npc.Controller = new NpcController(npc) { CustomSetAI = AILevels.SPACEBALL };
            npc.Controller.Initiate();
            Console.WriteLine("Created spaceball @4-4");
        }

        public void CreateBinaryBot(Vector position, bool santa = false)
        {
            var id = GetNextAvailableId();
            var tickId = -1;
            var ship = World.StorageManager.Ships[104];
            var npc = new EventNpc(id, ship.Name,
                new Hangar(0, ship, new Dictionary<int, Drone>(), position, this, ship.Health, ship.Nanohull),
                0, position, this, ship.Health, 0, ship.Shield,
                ship.Shield, ship.Health, ship.Damage, ship.Reward, 90);
            if (santa)
            {
                npc.Hangar.ShipDesign = World.StorageManager.Ships[32];
            }
            if (Entities.ContainsKey(npc.Id))
            {
                if (Properties.Server.DEBUG)
                    Console.WriteLine(id + "#Failed adding NPC");
                return;
            }

            AddEntity(npc);

            if (!Global.TickManager.Exists(npc))
            {
                Global.TickManager.Add(npc, out tickId);
                npc.SetTickId(tickId);
            }

            npc.Controller = new NpcController(npc) { CustomSetAI = AILevels.AGGRESSIVE };
            npc.Controller.Initiate();
        }
        #endregion

        #region Zones

        public void CreateDemiZone(Vector topLeft, Vector botRight)
        {
            var id = GetNextZoneId();
            if (!Pvp)
                Zones.Add(id, new DemiZone(id, topLeft, botRight, Faction.NONE));
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
            Out.WriteLog("Loaded objects on mapId " + Id);
        }

        public void DisablePortals(List<int> destinations = null)
        {
            foreach (var portal in Objects.Where(x => x.Value is Jumpgate))
            {
                var p = portal.Value as Jumpgate;
                if (destinations != null && destinations.Contains(p.DestinationMapId))
                {
                    DisablePortal(portal.Key, "Maps disabled for reconstruction");
                }
                else if (destinations == null) DisablePortal(portal.Key, "Maps disabled for reconstruction");
            }
        }

        public void CreatePortal(int map, int x, int y, int newX, int newY, int vwId = 0)
        {
            var id = GetNextObjectId();
            AddObject(new Jumpgate(id, 0, new Vector(x, y), this, new Vector(newX, newY), map, true, 0, 0, PortalGraphics.STANDARD_GATE));

            var zoneId = GetNextZoneId();
            if (!Pvp)
                Zones.Add(zoneId, new DemiZone(zoneId, new Vector(x - 500, y + 500), new Vector(x + 500, y - 500), Faction.NONE));
            Out.WriteLog("Created Portal on mapId " + Id);
        }

        public void DisablePortal(int id, string disableMsg = "")
        {
            var portal = Objects.FirstOrDefault(x => x.Key == id).Value as Jumpgate;
            portal?.Disable(disableMsg);
        }

        public void CreateLoW(Vector pos)
        {
            var id = GetNextObjectId();
            AddObject(new LowPortal(id, pos, this, 0) { Working = true });

            var zoneId = GetNextZoneId();
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(pos.X - 500, pos.Y + 500), new Vector(pos.X + 500, pos.Y - 500),Faction.NONE));
        }

        public void CreateGalaxyGates(Player player)
        {
            var id = GetNextObjectId();
            switch (Id)
            {
                case 1:
                    if (player.Gates.AlphaReady)
                        AddObject(new GalaxyGatePortal(player, id, 1, new Vector(2700, 700), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_1));
                    id = GetNextObjectId();
                    if (player.Gates.BetaReady)
                        AddObject(new GalaxyGatePortal(player, id, 2, new Vector(2300, 1800), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_2));
                    id = GetNextObjectId();
                    if (player.Gates.GammaReady)
                        AddObject(new GalaxyGatePortal(player, id, 3, new Vector(1500, 3000), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_3));
                    id = GetNextObjectId();
                    if (player.Gates.DeltaReady)
                        AddObject(new GalaxyGatePortal(player, id, 4, new Vector(4400, 3600), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_4));
                    if (player.Gates.EpsilonReady)
                        AddObject(new GalaxyGatePortal(player, id, 5, new Vector(4000, 1300), this, new Vector(10400, 6400), 51, PortalGraphics.EPSILON_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.ZetaReady)
                        AddObject(new GalaxyGatePortal(player, id, 6, new Vector(2500, 3500), this, new Vector(10400, 6400), 51, PortalGraphics.ZETA_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.KappaReady)
                        AddObject(new GalaxyGatePortal(player, id, 7, new Vector(5200, 2600), this, new Vector(10400, 6400), 51, PortalGraphics.KAPPA_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.KronosReady)
                        AddObject(new GalaxyGatePortal(player, id, 8, new Vector(7400, 4200), this, new Vector(10400, 6400), 51, PortalGraphics.KRONOS_GATE));
                    break;
                case 5:
                    if (player.Gates.AlphaReady)
                        AddObject(new GalaxyGatePortal(player, id, 1, new Vector(17300, 1000), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_1));
                    id = GetNextObjectId();
                    if (player.Gates.BetaReady)
                        AddObject(new GalaxyGatePortal(player, id, 2, new Vector(18000, 2300), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_2));
                    id = GetNextObjectId();
                    if (player.Gates.GammaReady)
                        AddObject(new GalaxyGatePortal(player, id, 3, new Vector(19100, 3600), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_3));
                    id = GetNextObjectId();
                    if (player.Gates.DeltaReady)
                        AddObject(new GalaxyGatePortal(player, id, 4, new Vector(15100, 1700), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_4));
                    if (player.Gates.EpsilonReady)
                        AddObject(new GalaxyGatePortal(player, id, 5, new Vector(18600, 4200), this, new Vector(10400, 6400), 51, PortalGraphics.EPSILON_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.ZetaReady)
                        AddObject(new GalaxyGatePortal(player, id, 6, new Vector(16200, 1600), this, new Vector(10400, 6400), 51, PortalGraphics.ZETA_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.KappaReady)
                        AddObject(new GalaxyGatePortal(player, id, 7, new Vector(15900, 2800), this, new Vector(10400, 6400), 51, PortalGraphics.KAPPA_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.KronosReady)
                        AddObject(new GalaxyGatePortal(player, id, 8, new Vector(10114, 5800), this, new Vector(10400, 6400), 51, PortalGraphics.KRONOS_GATE));
                    break;
                case 9:
                    if (player.Gates.AlphaReady)
                        AddObject(new GalaxyGatePortal(player, id, 1, new Vector(17500, 11500), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_1));
                    id = GetNextObjectId();
                    if (player.Gates.BetaReady)
                        AddObject(new GalaxyGatePortal(player, id, 2, new Vector(18000, 10500), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_2));
                    id = GetNextObjectId();
                    if (player.Gates.GammaReady)
                        AddObject(new GalaxyGatePortal(player, id, 3, new Vector(19000, 10000), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_3));
                    id = GetNextObjectId();
                    if (player.Gates.DeltaReady)
                        AddObject(new GalaxyGatePortal(player, id, 4, new Vector(15500, 9000), this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_4));
                    id = GetNextObjectId();
                    if (player.Gates.EpsilonReady)
                        AddObject(new GalaxyGatePortal(player, id, 5, new Vector(18000, 9000), this, new Vector(10400, 6400), 51, PortalGraphics.EPSILON_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.ZetaReady)
                        AddObject(new GalaxyGatePortal(player, id, 6, new Vector(16000, 10800), this, new Vector(10400, 6400), 51, PortalGraphics.ZETA_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.KappaReady)
                        AddObject(new GalaxyGatePortal(player, id, 7, new Vector(16700, 9600), this, new Vector(10400, 6400), 51, PortalGraphics.KAPPA_GATE));
                    id = GetNextObjectId();
                    if (player.Gates.KronosReady)
                        AddObject(new GalaxyGatePortal(player, id, 8, new Vector(13500, 7800), this, new Vector(10400, 6400), 51, PortalGraphics.KRONOS_GATE));
                    break;
            }
        }

        public void CreateGalaxyGate(Player player, int ggId, Vector position)
        {
            var id = GetNextObjectId();
            switch (ggId)
            {
                case 1:
                    AddObject(new GalaxyGatePortal(player, id, 1, position, this, new Vector(10400, 6400), 51, PortalGraphics.GALAXYGATE_1));
                    break;
                case 2:
                    AddObject(new GalaxyGatePortal(player, id, 2, position, this, new Vector(10400, 6400), 52, PortalGraphics.GALAXYGATE_2));
                    break;
                case 3:
                    AddObject(new GalaxyGatePortal(player, id, 3, position, this, new Vector(10400, 6400), 53, PortalGraphics.GALAXYGATE_3));
                    break;
            }
        }

        public void CreateExitGate(Player player, GalaxyGate gate, Vector position)
        {
            var id = GetNextObjectId();
            var baseLocation = player.GetClosestStation(true);
            AddObject(new ExitGatePortal(gate, id, position, this, baseLocation.Item1, baseLocation.Item2.Id));
        }

        public void CreateHiddenPortal(int map, Vector pos, Vector newPos, int vwId = 0)
        {
            var id = GetNextObjectId();
            AddObject(new Jumpgate(id, 0, pos, this, newPos, map, false, 0, 0, 0));
            Out.WriteLog("Created Portal on mapId " + Id);
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
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(position.X - 1000, position.Y + 1000), new Vector(position.X + 1000, position.Y - 1000), faction));
            Out.WriteLog("Created Station on mapId " + Id);
        }

        public void CreatePirateStation(Vector vector)
        {
            var id = GetNextObjectId();
            AddObject(new PirateStation(id, vector, this));

            var zoneId = GetNextZoneId();
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(vector.X - 3000, vector.Y + 3000), new Vector(vector.X + 3000, vector.Y - 3000), Faction.NONE));

            Out.WriteLog("Created Pirate Station on mapId " + Id);
        }

        public void CreateHealthStation(Vector vector)
        {
            var id = GetNextObjectId();
            AddObject(new HealthStation(id, vector, this));
            Out.WriteLog("Created Health Station on mapId " + Id);
        }

        public void CreateRelayStation(Vector vector)
        {
            var id = GetNextObjectId();
            AddObject(new RelayStation(id, vector, this));
            Out.WriteLog("Created Relay Station on mapId " + Id);
        }

        public void CreateReadyRelayStation(Vector vector)
        {
            var id = GetNextObjectId();
            AddObject(new ReadyRelayStation(id, vector, this));
            Out.WriteLog("Created Relay Station on mapId " + Id);
        }

        public void CreateQuestGiver(Faction faction, Vector pos)
        {
            var id = GetNextObjectId();
            var questGiverId = World.StorageManager.QuestGivers.Count;
            var questGiver = new QuestGiver(id, questGiverId, faction, pos, this);
            World.StorageManager.QuestGivers.Add(questGiverId, questGiver);
            AddObject(questGiver);
            Out.WriteLog("Created Quest Giver on mapId " + Id);
        }

        public void CreateAsteroid(string name, Vector pos)
        {
            var id = GetNextObjectId();
            var bStation = World.StorageManager.ClanBattleStations.Count;
            World.StorageManager.ClanBattleStations.Add(bStation, null);
            AddObject(new Asteroid(id, bStation, name, pos, this));
            Out.WriteLog("Created Asteroid on mapId " + Id);
        }

        public void CreateAdvertisementBanner(short advertiser, Vector pos)
        {
            var id = GetNextObjectId();
            AddObject(new Billboard(id, pos, this, advertiser));
        }

        public void CreatePirateGate(Faction faction, Vector pos, int destinationMapId, Vector destination, bool isBroken, bool nocopy = false)
        {
            var id = GetNextObjectId();
            PirateGate gate = new PirateGate(id, faction, pos, this, destinationMapId, destination, isBroken);

            AddObject(gate);
            if (!isBroken && !nocopy)
            {
                var destinationMap = World.StorageManager.Spacemaps[destinationMapId];
                destinationMap.CreatePirateGate(faction, destination, Id, pos, true);
            }

            var zoneId = GetNextZoneId();
            Zones.Add(zoneId, new DemiZone(zoneId, new Vector(pos.X - 500, pos.Y + 500), new Vector(pos.X + 500, pos.Y - 500), Faction.NONE));
        }

        #endregion

        #region Collectables

        public void CreateOre(OreTypes type, Vector pos, Vector[] limits)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new RegularOre(id, hash, type, pos, this, limits);
            HashedObjects[hash] = box;
            if(AddObject(box))
                Out.WriteLog("Created Ore["+type+"] on mapId " + Id);
        }

        public void CreateBox(Types type, Vector pos, Vector[] limits)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var honeyBox = HashedObjects[hash] is FakeHoneyBox;
            if (honeyBox)
            {
                Debug.WriteLine("Honey box created: " + HashedObjects.Keys.ToList()[id]);
            }
            var box = new BonusBox(id, hash, type, pos, this, limits,true, honeyBox);
            HashedObjects[hash] = box;
            if (AddObject(box))
                Out.WriteLog("Created Box[" + type + "] on mapId " + Id);
        }

        public void CreateUcbBox(Types type, Vector pos, Vector[] limits)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var honeyBox = HashedObjects[hash] is FakeHoneyBox;
            if (honeyBox)
            {
                Debug.WriteLine("Honey box created: " + HashedObjects.Keys.ToList()[id]);
            }
            var box = new BonusBox(id, hash, type, pos, this, limits, true, honeyBox, new Reward(RewardType.AMMO, Item.Find("ammunition_laser_ucb-100"), 1000));
            HashedObjects[hash] = box;
            if (AddObject(box))
                Out.WriteLog("Created UCB Box[" + type + "] on mapId " + Id);
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
                    Out.WriteLog($"Created cargo loot ({position.X},{position.Y}) on mapId " + Id);
            }
        }

        public void CreateLootBox(Vector position, Reward reward, Types type, int disposeMs)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new LootBox(id, hash, type, position, this, reward, disposeMs);
            HashedObjects[hash] = box;
            if (AddObject(box))
                Out.WriteLog("Created LootBox [" + type + "] on mapId " + Id);
        }

        public void CreatePirateBooty(Types type, Vector pos, Vector[] limits)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new PirateBooty(id, hash, type, pos, this, limits, true);
            HashedObjects[hash] = box;
            if (AddObject(box))
                Out.WriteLog("Created Pirate Booty [" + type + "] on mapId " + Id);
        }

        public void CreateEventBox(Types type, Vector pos, Vector[] limits)
        {
            var id = GetNextObjectId();
            var hash = HashedObjects.Keys.ToList()[id];
            var box = new EventBox(id, hash, type, pos, this, limits, true);
            HashedObjects[hash] = box;
            if (AddObject(box))
                Out.WriteLog("Created Event Box[" + type + "] on mapId " + Id);
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
            if (Id == 93)
            {
                var zoneId = GetNextZoneId();
                PalladiumZone zone = new PalladiumZone1(zoneId);
                Zones.Add(zoneId, zone);
                CreatePOI(new POI("Field_03", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE,
                    new List<Vector>
                    {
                        new Vector(12000, 17500), new Vector(31000, 25000)
                    }));
                for (var i = 0; i < 250; i++)
                    CreateOre(OreTypes.PALLADIUM, Vector.Random(this, zone.TopLeft, zone.BottomRight), new Vector[] { zone.TopLeft, zone.BottomRight });

            }

            if (Id == 91)
            {
                var zoneId = GetNextZoneId();
                PalladiumZone zone = new PalladiumZone2(zoneId);
                Zones.Add(zoneId, zone);
                CreatePOI(new POI("Field_01", objects.world.map.pois.Types.GENERIC, Designs.NEBULA, Shapes.RECTANGLE,
                    new List<Vector>
                    {
                        new Vector(19000, 11000), new Vector(23500, 14600)
                    }));
                for (var i = 0; i < 50; i++)
                    CreateOre(OreTypes.PALLADIUM, Vector.Random(this, zone.TopLeft, zone.BottomRight), new Vector[] { zone.TopLeft, zone.BottomRight });

            }
        }

        public void CreateMineField()
        {
            if (Id == 91)
            {
                var zoneId = GetNextZoneId();
                var mineZone = new MineZone(zoneId, new Vector(15000, -1), new Vector(28500, 6500),
                    this);
                mineZone.Spawn();
                Zones.Add(zoneId, mineZone);
                zoneId = GetNextZoneId();
                mineZone = new MineZone(zoneId, new Vector(14500, 20000), new Vector(27000, 26000),
                    this);
                mineZone.Spawn();
                Zones.Add(zoneId, mineZone);
            }
            else if (Id == 93)
            {
                var zoneId = GetNextZoneId();
                var mineZone = new MineZone(zoneId, new Vector(700, 20500), new Vector(10700, 25700),
                    this);
                mineZone.Spawn();
                Zones.Add(zoneId, mineZone);
                zoneId = GetNextZoneId();
                mineZone = new MineZone(zoneId, new Vector(5000, 5500), new Vector(10000, 20400),
                    this);
                mineZone.Spawn();
                Zones.Add(zoneId, mineZone);
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
