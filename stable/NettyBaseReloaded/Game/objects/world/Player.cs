using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.pets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.storages.playerStorages;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.objects.world
{
    class Player : Character
    {
        /**********
         * BASICS *
         **********/
        public string SessionId { get; set; }

        public Rank RankId { get; set; }
        public new PlayerController Controller { get; set; }

        /***************
         * INFORMATION *
         ***************/
        public Clan Clan { get; set; }

        public Group Group { get; set; }
        public Level Level { get; set; }
        public long Experience { get; set; }
        public long Honor { get; set; }
        public double Credits { get; set; }
        public double Uridium { get; set; }
        public int GGSpins { get; set; }
        public float Jackpot { get; set; }

        public int[] BootyKeys { get; set; }

        /*********
         * EXTRA *
         *********/
        private int _rings;

        public int Rings
        {
            get { return _rings; }
            //64 => Kronos (Max value)
            set { _rings = (value <= 64) ? value : 64; }
        }

        public bool Premium { get; set; }
        public int CurrentConfig { get; set; }

        public Ammo CurrentAmmo { get; set; }
        public Ammo SpentAmmo { get; set; }

        public bool SoftBan { get; set; }
        public bool InEquipmentArea { get; set; }
        public bool InTradeArea { get; set; }
        public bool InDemiZone { get; set; }
        public bool InRadiationArea { get; set; }
        public bool InPortalArea { get; set; }
        public bool InInstaRepairZone { get; set; }
        public int JumpVouchers { get; set; }

        public List<DroneFormation> OwnedFormations { get; set; }
        public bool Warping { get; set; }

        public Pet Pet { get; set; }

        public List<Booster> Boosters { get; set; }

        public Settings Settings { get; set; }

        public RocketLauncher RocketLauncher { get; set; }

        public StatsStorage Stats { get; set; }

        public PlayerStorage UserStorage { get; set; }

        public SettingsStorage SettingsStorage = new SettingsStorage();

        public EntitiesStorage EntitiesStorage = new EntitiesStorage();


        /*********
         * STATS *
         *********/
        public override int MaxHealth
        {
            get
            {
                var value = Hangar.Ship.Health;

                switch (Formation)
                {
                    case DroneFormation.DIAMOND:
                        value = (int) (value * 0.7); //-30%
                        break;
                    case DroneFormation.MOTH:
                    case DroneFormation.HEART:
                        value = (int) (value * 1.2); // +20%
                        break;
                }
                value = (int) (value * Hangar.Ship.GetHealthBonus(this));

                return value;
            }
        }

        public override int MaxShield
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].MaxShield;

                switch (Formation)
                {
                    case DroneFormation.TURTLE:
                        value = (int) (value * 1.1); //+10%
                        break;
                    case DroneFormation.DOUBLE_ARROW:
                        value = (int) (value * 0.8); //-20%
                        break;

                }
                value = (int) (value * Hangar.Ship.GetShieldBonus(this));

                return value;
            }
        }

        public override int CurrentShield
        {
            get { return Hangar.Configurations[CurrentConfig - 1].CurrentShield; }
            set { Hangar.Configurations[CurrentConfig - 1].CurrentShield = value; }
        }

        public override double ShieldAbsorption
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].ShieldAbsorbation;
                switch (Formation)
                {
                    case DroneFormation.CRAB:
                        value += 0.2;
                        break;

                }

                return value;
            }
        }

        public override double ShieldPenetration
        {
            get
            {
                switch (Formation)
                {
                    case DroneFormation.MOTH:
                        return 0.2;
                    case DroneFormation.PINCER:
                        return -0.1;
                    case DroneFormation.HEART:
                    case DroneFormation.DOUBLE_ARROW:
                        return 0.1;
                    default:
                        return 0;
                }
            }
        }

        public override int Speed
        {
            get { return Hangar.Ship.Speed + Hangar.Configurations[CurrentConfig - 1].Speed; }
        }

        public override int Damage
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].Damage;
                switch (Formation)
                {
                    case DroneFormation.TURTLE:
                        value = (int) (value * 0.925); //-7.5%
                        break;
                    case DroneFormation.ARROW:
                        value = (int) (value * 0.97); //-3%
                        break;
                    case DroneFormation.PINCER:
                        value = (int) (value * 1.03); //+3%
                        break;
                    case DroneFormation.HEART:
                        value = (int) (value * 0.95); //-5%
                        break;

                }
                value = (int) (value * Hangar.Ship.GetDamageBonus(this));
                return value;
            }
        }

        public override int RocketDamage
        {
            get
            {
                var value = 1000;
                switch (Formation)
                {
                    case DroneFormation.TURTLE:
                        value = (int) (value * 0.925); //-7.5%
                        break;
                    case DroneFormation.ARROW:
                        value = (int) (value * 1.2); //+20%
                        break;
                    case DroneFormation.STAR:
                        value = (int) (value * 1.25); //+25%
                        break;

                }

                return value;
            }
        }

        public List<Drone> Drones => Hangar.Drones;

        public Dictionary<int, Ability> Abilities { get; set; }

        public List<InvitedForGroup> InvitedForGroups { get; set; }

        public List<Npc> AttachedNpcs { get; set; }

        public List<GalaxyGate> AvailableGalaxyGates { get; set; }

        public Cargo Cargo { get; set; }

        /// <summary>
        /// This is a for the multi-client support.
        /// - Work in progress -
        /// </summary>
        public bool UsingNewClient { get; set; }

        public Player(int id, string name, string sessionId, Hangar hangar, Clan clan, Faction factionId, Reward reward,
            DropableRewards dropableRewards,
            Rank rankId, Level level, long experience, long honor, double credits, double uridium, float jackpot,
            int rings, bool premium, RocketLauncher launcher, Cargo cargo, StatsStorage stats)
            : base(id, name, hangar, factionId, hangar.Position, hangar.Spacemap, hangar.Health, hangar.Nanohull,
                reward, dropableRewards)
        {
            SessionId = sessionId;
            RankId = rankId;
            Level = level;
            Experience = experience;
            Honor = honor;
            Credits = credits;
            Uridium = uridium;
            Jackpot = jackpot;
            Rings = rings;
            Premium = premium;
            CurrentConfig = 1; //Selected config1 by default
            Clan = clan;
            RocketLauncher = launcher;
            //Cargo = cargo;
            Stats = stats;
            //Settings = settings;

            SoftBan = false;
            UsingNewClient = false;

            InDemiZone = false;
            InRadiationArea = false;
            InTradeArea = false;
            InEquipmentArea = false;
            InPortalArea = false;
            InInstaRepairZone = false;

            UserStorage = new PlayerStorage(Id, Credits, Uridium);

            Boosters = new List<Booster>();
            BootyKeys = new[] {2, 2, 1};
            Abilities = new Dictionary<int, Ability>();
            Pet = null;
            Group = null;
            InvitedForGroups = new List<InvitedForGroup>();
            AttachedNpcs = new List<Npc>();
            OwnedFormations = new List<DroneFormation>();
            Cargo = new Cargo(0,0,0,0,0,0,0,0,0,0);
            Settings = new Settings();
        }

        public int GetRewardLevel()
        {
            int baseRewardMultiplyer = 21;
            if (Level.Id < 20)
            {
                return baseRewardMultiplyer - Level.Id;
            }
            return 1;
        }

        public void Tick()
        {
            BasicDbRefresh();
            LevelChecker();
            RefreshBonuses();
            //BetaTestCountdown();
            Hangar.DronesLevelChecker(this);
        }

        private DateTime LastTimeAnnounced = new DateTime(2017, 1, 21, 0, 0, 0);

        public void BetaTestCountdown()
        {
            if (!SettingsStorage.BetaCountdown && LastTimeAnnounced < DateTime.Now.AddSeconds(10) && Properties.Server.PUBLIC_BETA_END > DateTime.Now ||
                Properties.Server.PUBLIC_BETA_END < DateTime.Now) return;

            LastTimeAnnounced = DateTime.Now;

            var gameSession = World.StorageManager.GetGameSession(Id);
            var timeLeft = Properties.Server.PUBLIC_BETA_END - DateTime.Now;
            var client = World.StorageManager.GetGameSession(Id).Client;
            if (client != null)
            {
                if (timeLeft.Days > 1) Packet.Builder.LegacyModule(gameSession, "0|A|STD|" + timeLeft.Days + " days and " + timeLeft.Hours + " hours of testing left.");
                else if (timeLeft.Minutes > 1) Packet.Builder.LegacyModule(gameSession, "0|A|STD|" + timeLeft.Hours + " hours and " + timeLeft.Minutes + " minutes of testing left.");
                else Packet.Builder.LegacyModule(gameSession, "0|A|STD|Thank you for coming! Hope you had a great time =)");
            }
        }

        public void LevelChecker()
        {
            if (!World.StorageManager.Levels.PlayerLevels.ContainsKey(Level.Id + 1))
                return;

            foreach (var level in World.StorageManager.Levels.PlayerLevels)
            {
                if (Experience > level.Value.Experience && level.Key > Level.Id)
                    LevelUp();
            }
        }

        public void LevelUp()
        {
            if (!World.StorageManager.Levels.PlayerLevels.ContainsKey(Level.Id + 1))
                return;

            Level = World.StorageManager.Levels.PlayerLevels[Level.Id + 1];
            //World.StorageManager.GetGameSession(Id).Client.Send(Builder.LegacyModule(
            //    "0|A|LUP|" + Level.Id + "|" + World.StorageManager.Levels.PlayerLevels[Level.Id + 1].Experience).Bytes);
            Refresh();
        }

        public void Refresh()
        {
            Packet.Builder.ShipInitializationCommand(World.StorageManager.GetGameSession(Id));
        }

        public DateTime LastDbRefreshTime = new DateTime(2017, 1, 13, 0, 0, 0);

        public void BasicDbRefresh(bool instantRefresh = false)
        {
            if (LastDbRefreshTime.AddSeconds(11) > DateTime.Now && !instantRefresh) return;
            LastDbRefreshTime = DateTime.Now;

            World.DatabaseManager.BasicRefresh(this);
            UserStorage.PlayerRefresh(this);
        }

        public DateTime LastSaveTime = new DateTime(2016, 12, 24, 0, 0, 0);

        public void InstantSave()
        {
            World.DatabaseManager.BasicSave(this);
            LastSaveTime = DateTime.Now;
        }

        public string GetConsumablesPacket()
        {
            bool rep = false;
            bool droneRep = false;
            bool ammoBuy = false;
            bool cloak = false;
            bool tradeDrone = false;
            bool smb = false;
            bool ish = false;
            bool aim = false;
            bool autoRocket = false;
            bool autoRocketLauncer = false;
            bool rocketBuy = false;
            bool jump = false;
            bool petRefuel = false;
            bool jumpToBase = false;

            var currConfig = Hangar.Configurations[CurrentConfig - 1];
            if (currConfig.Consumables != null &&
                currConfig.Consumables.Count > 0)
            {
                foreach (var item in currConfig.Consumables)
                {
                    var slotbarItem = Settings.Slotbar._items[item.Value.LootId];
                    if (slotbarItem != null)
                    {
                        slotbarItem.CounterValue = item.Value.Amount;
                        slotbarItem.Visible = true;
                        if (UsingNewClient)
                        {
                            World.StorageManager.GetGameSession(Id)?.Client.Send(slotbarItem.ChangeStatus());
                        }
                    }

                    switch (item.Key)
                    {
                        case "equipment_extra_cpu_ajp-01":
                            jump = true;
                            break;
                        case "equipment_extra_repbot_rep-s":
                        case "equipment_extra_repbot_rep-1":
                        case "equipment_extra_repbot_rep-2":
                        case "equipment_extra_repbot_rep-3":
                        case "equipment_extra_repbot_rep-4":
                            rep = true;
                            break;
                        case "equipment_extra_cpu_smb-01":
                            smb = true;
                            break;
                        case "equipment_extra_cpu_ish-01":
                            ish = true;
                            break;
                        case "equipment_extra_cpu_aim-01":
                        case "equipment_extra_cpu_aim-02":
                            aim = true;
                            break;
                        case "equipment_extra_cpu_jp-01":
                        case "equipment_extra_cpu_jp-02":
                            jumpToBase = true;
                            break;
                        case "equipment_extra_cpu_cl04k-xl":
                        case "equipment_extra_cpu_cl04k-m":
                        case "equipment_extra_cpu_cl04k-xs":
                            cloak = true;
                            break;
                        case "equipment_extra_cpu_arol-x":
                            autoRocket = true;
                            break;
                        case "equipment_extra_cpu_rllb-x":
                            autoRocketLauncer = true;
                            break;
                        case "equipment_extra_cpu_dr-01":
                        case "equipment_extra_cpu_dr-02":
                            droneRep = true;
                            break;
                    }
                }
            }

            return Convert.ToInt32(droneRep) + "|0|" + Convert.ToInt32(jumpToBase) + "|" +
                   Convert.ToInt32(ammoBuy) + "|" + Convert.ToInt32(rep) + "|" + Convert.ToInt32(tradeDrone) +
                   "|0|" + Convert.ToInt32(smb) + "|" + Convert.ToInt32(ish) + "|0|" + Convert.ToInt32(aim) + "|" +
                   Convert.ToInt32(autoRocket) + "|" + Convert.ToInt32(cloak) + "|" +
                   Convert.ToInt32(autoRocketLauncer) + "|" + Convert.ToInt32(rocketBuy) + "|" +
                   Convert.ToInt32(jump) + "|" + Convert.ToInt32(petRefuel);

        }

        public int LaserCount()
        {
            return Hangar.Configurations[CurrentConfig - 1].LaserCount;
        }

        public void RefreshBonuses()
        {
        }

        public bool IsOnHomeMaps()
        {
            switch (Spacemap.Id)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    if (FactionId == Faction.MMO) return true;
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                    if (FactionId == Faction.EIC) return true;
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    if (FactionId == Faction.VRU) return true;
                    break;
            }
            return false;
        }

        public Zone GetCurrentZone()
        {
            throw new NotImplementedException();
        }

        public string GetRobot()
        {
            return "equipment_extra_repbot_rep-4";
        }

        public int GetRobotLevel()
        {
            return 4;
        }

        public int GetCpuUsesLeft(PlayerController.CPU.Types type)
        {
            var consumables = Hangar.Configurations[CurrentConfig - 1].Consumables;
            switch (type)
            {
                case PlayerController.CPU.Types.CLOAK:
                    if (consumables.ContainsKey("equipment_extra_cpu_cl04k-xl"))
                        return consumables["equipment_extra_cpu_cl04k-xl"].Amount;
                    if (consumables.ContainsKey("equipment_extra_cpu_cl04k-m"))
                        return consumables["equipment_extra_cpu_cl04k-m"].Amount;
                    if (consumables.ContainsKey("equipment_extra_cpu_cl04k-xs"))
                        return consumables["equipment_extra_cpu_cl04k-xs"].Amount;
                    break;
            }
            return 0;
        }

        public Tuple<Vector, Spacemap> GetClosestStation()
        {
            if (Properties.Game.PVP_MODE)
            {
                var map = World.StorageManager.Spacemaps[16];
                var stations = map.Objects.Values.Where(x => x is Station);
                foreach (var station in stations)
                {
                    var pStation = station as Station;
                    if (pStation.Faction == FactionId)
                    {
                        return new Tuple<Vector, Spacemap>(pStation.Position, map);
                    }
                }
            }
            throw new NotImplementedException();
        }

        public void CleanRange()
        {
            RangeObjects.Clear();
            RangeEntities.Clear();
            RangeZones.Clear();
            RangeCollectables.Clear();
        }

        public void CleanStorage()
        {
            EntitiesStorage.LoadedObjects.Clear();
            EntitiesStorage.LoadedPOI.Clear();
        }

        public void LoadObject(Object obj)
        {
            if (obj is Station) LoadStation(obj as Station);
            if (obj is Jumpgate) LoadPortal(obj as Jumpgate);
        }

        public void LoadPOI(POI poi)
        {
            var gameSession = World.StorageManager.GetGameSession(Id);
            EntitiesStorage.LoadedPOI.Add(poi.Id, poi);
            Packet.Builder.MapAddPOICommand(gameSession, poi);
        }

        private void LoadStation(Station station)
        {
            var gameSession = World.StorageManager.GetGameSession(Id);
            EntitiesStorage.LoadedObjects.Add(station.Id, station);
            Packet.Builder.StationCreateCommand(gameSession, station);
        }

        private void LoadPortal(Jumpgate portal)
        {
            var gameSession = World.StorageManager.GetGameSession(Id);
            Console.WriteLine("Adding");
            EntitiesStorage.LoadedObjects.Add(portal.Id, portal);
            Console.WriteLine("Sending");
            Packet.Builder.JumpgateCreateCommand(gameSession,portal);
        }

        public void SetPosition(Vector targetPosition)
        {
            if (Moving)
                MovementController.Move(this, MovementController.ActualPosition(this));
            Position = targetPosition;
            Refresh();
        }

        public void ClickableCheck(Object obj)
        {
            if (obj is IClickable)
            {
                var active = Vector.IsInRange(Position, obj.Position, 1000);
                Packet.Builder.MapAssetActionAvailableCommand(World.StorageManager.GetGameSession(Id), obj, active);
            }
        }
    }
}
