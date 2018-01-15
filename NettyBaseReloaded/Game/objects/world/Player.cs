using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.controllers.login;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.extra;
using NettyBaseReloaded.Game.objects.world.players.extra.boosters;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using ClanRelationModule = NettyBaseReloaded.Game.netty.commands.new_client.ClanRelationModule;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;
using Range = NettyBaseReloaded.Game.objects.world.characters.Range;
using State = NettyBaseReloaded.Game.objects.world.players.State;

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

        public Equipment Equipment { get; private set; }

        public Statistics Statistics { get; private set; }

        public Information Information { get; private set; }

        public State State { get; private set; }

        private Hangar _hangar;

        public override Hangar Hangar
        {
            get
            {
                if (Equipment?.Hangars?[Equipment.ActiveHangar] != null)
                {
                    return Equipment.Hangars[Equipment.ActiveHangar];
                }
                return _hangar;
            }
            set
            {
                if (Equipment?.Hangars?[Equipment.ActiveHangar] != null)
                {
                    Equipment.Hangars[Equipment.ActiveHangar] = value;
                }
                _hangar = value;
            }
        }

        /*********
         * EXTRA *
         *********/

        public Pet Pet { get; set; }

        public Settings Settings { get; private set; }

        public Storage Storage { get; private set; }

        public PlayerLog Log { get; private set; }

        public List<Booster> Boosters { get; set; }

        public ConcurrentDictionary<Player, Booster> InheritedBoosters = new ConcurrentDictionary<Player, Booster>();

        public override Skilltree Skills { get; set; }

        public Group Group { get; set; }

        public List<Tech> Techs { get; set; }

        /*********
         * STATS *
         *********/

        public int CurrentConfig { get; set; }

        public override int MaxHealth
        {
            get
            {
                var value = Hangar.Ship.Health;

                switch (Formation)
                {
                    case DroneFormation.CHEVRON:
                        value = (int) (value * 0.2); // -20%
                        break;
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
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].CurrentShield;
                switch (Formation)
                {
                    case DroneFormation.HEART:
                        value = (int)(value * 1.1); // +10%
                        break;
                }
                return value;
            }
            set { Hangar.Configurations[CurrentConfig - 1].CurrentShield = value; }
        }

        public override double ShieldAbsorption
        {
            get
            {
                var value = (double)Hangar.Configurations[CurrentConfig - 1].ShieldAbsorbation / MaxShield;
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

        public double BoostedAcceleration = 0;
        public override int Speed
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].Speed;
                if (BoostedAcceleration > 0)
                    value = (int) (value * (1 + BoostedAcceleration));
                return value;
            }
        }

        public double BoostedDamage = 0;
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

                if (BoostedDamage > 0)
                    value = (int) (value * Hangar.Ship.GetDamageBonus(this) * (1 + BoostedDamage));
                else value = (int) (value * Hangar.Ship.GetDamageBonus(this));
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
                    case DroneFormation.CHEVRON:
                        value = (int)(value * 1.5); //+50%
                        break;
                }

                return value;
            }
        }

        public Dictionary<string, Extra> Extras
        {
            get { return Hangar.Configurations[CurrentConfig - 1].Extras; }
        }

        public override RocketLauncher RocketLauncher
        {
            get { return Hangar.Configurations[CurrentConfig - 1].RocketLauncher; }
        }

        /// <summary>
        /// This is a for the multi-client support.
        /// - Work in progress -
        /// </summary>
        public bool UsingNewClient { get; set; }

        /// <summary>
        /// Lists
        /// </summary>
        public List<Drone> Drones => Hangar.Drones;

        public List<Npc> AttachedNpcs = new List<Npc>();

        public Dictionary<int, PlayerEvent> EventsPraticipating = new Dictionary<int, PlayerEvent>();

        public Player(int id, string name, Clan clan, Hangar hangar, int currentHealth, int currentNano,
            Faction factionId, Vector position, Spacemap spacemap, Reward rewards,
            string sessionId, Rank rankId, bool usingNewClient = false) : base(id, name, hangar, factionId, position,
            spacemap, rewards, clan)
        {
            InitializeClasses();
            SessionId = sessionId;
            RankId = rankId;
            UsingNewClient = usingNewClient;
            CurrentConfig = 1;
            CurrentHealth = currentHealth;
            CurrentNanoHull = currentNano;
        }

        public new void Tick()
        {
            // TODO -> Added ticked processes
            if (!Controller.Active || EntityState == EntityStates.DEAD)
                return;
            LevelChecker();
            Storage.Tick();
            TickBoosters();
            AssembleEnemyWarn();
            Information.Timer();
            State.Tick();
            Hangar.DronesLevelChecker(this);
            Group?.Tick();
            TickEvents();
            //TickTechs();
        }

        private void TickTechs()
        {
            foreach (var tech in Techs)
            {
                tech.Tick();
            }
        }

        private void TickEvents()
        {
            foreach (var gameEvent in EventsPraticipating.Values)
            {
                gameEvent.Tick();
            }
        }

        private void InitializeClasses()
        {
            Equipment = new Equipment(this);
            Statistics = World.DatabaseManager.LoadStatistics(this);
            Information = new Information(this);
            State = new State(this);
            Skills = World.DatabaseManager.LoadSkilltree(this);
            Storage = new Storage(this);
            Log = new PlayerLog(SessionId);
            Boosters = new List<Booster>(); // TODO: Load from SQL
            Settings = new Settings(this);
            Range.EntityAdded += CharacterEnteredRange;
            Range.EntityRemoved += CharacterExitedRange;
        }

        public void ClickableCheck(Object obj)
        {
            if (obj is IClickable)
            {
                var active = Vector.IsInRange(Position, obj.Position, obj.Range);
                Packet.Builder.MapAssetActionAvailableCommand(World.StorageManager.GetGameSession(Id), obj, active);
            }
        }

        public void LoadObject(Object obj)
        {
            if (obj == null) return;

            if (obj is Station) Storage.LoadStation(obj as Station);
            else if (obj is Jumpgate) Storage.LoadPortal(obj as Jumpgate);
            else if (obj is Asteroid) Storage.LoadAsteroid(obj as Asteroid);
            else if (obj is Asset) Storage.LoadAsset(obj as Asset);
            else if (obj is Collectable) Storage.LoadCollectable(obj as Collectable);
            else if (obj is Ore) Storage.LoadResource(obj as Ore);
        }

        public void UnloadObject(Object obj)
        {
            if (obj is Collectable) Storage.UnLoadCollectable(obj as Collectable);
            else
            {
                if (Storage.LoadedObjects.ContainsKey(obj.Id))
                    Storage.LoadedObjects.Remove(obj.Id);
            }
        }

        private DateTime LastConfigSave = new DateTime();
        public void SaveConfig()
        {
            if (LastConfigSave.AddSeconds(5) < DateTime.Now)
                World.DatabaseManager.SaveConfig(this);
        }

        public void Save()
        {
            World.DatabaseManager.SavePlayerHangar(this);
            World.DatabaseManager.SaveConfig(this);
        }

        public void Refresh()
        {
            var gameSession = World.StorageManager.GetGameSession(Id);
            if (gameSession == null) return;
            Packet.Builder.ShipInitializationCommand(gameSession);
            ILogin.SendLegacy(gameSession);
        }

        public void SetPosition(Vector targetPosition)
        {
            Position = targetPosition;
            OldPosition = targetPosition;
            Destination = targetPosition;
            Direction = targetPosition;
            MovementStartTime = DateTime.Now;
            MovementTime = 0;
            Moving = false;
            Refresh();
            MovementController.Move(this, MovementController.ActualPosition(this));
        }

        public Tuple<Vector, Spacemap> GetClosestStation()
        {
            Spacemap map = null;
            if (Properties.Game.PVP_MODE)
            {
                map = World.StorageManager.Spacemaps[16];
            }
            else
            {
                if (Spacemap?.Id > 16)
                {
                    switch (FactionId)
                    {
                        case Faction.MMO:
                            map = World.StorageManager.Spacemaps[20];
                            break;
                        case Faction.EIC:
                            map = World.StorageManager.Spacemaps[24];
                            break;
                        case Faction.VRU:
                            map = World.StorageManager.Spacemaps[28];
                            break;
                    }
                }
                else
                {
                    switch (FactionId)
                    {
                        case Faction.MMO:
                            map = World.StorageManager.Spacemaps[1];
                            break;
                        case Faction.EIC:
                            map = World.StorageManager.Spacemaps[5];
                            break;
                        case Faction.VRU:
                            map = World.StorageManager.Spacemaps[9];
                            break;

                    }
                }
            }

            var stations = map.Objects.Values.Where(x => x is Station);
            foreach (var station in stations)
            {
                var pStation = station as Station;
                if (pStation?.Faction == FactionId)
                {
                    return new Tuple<Vector, Spacemap>(pStation?.Position, map);
                }
            }
            return null;
        }

        public void SendLogMessage(string logMsg, LogMessage.LogType logType = LogMessage.LogType.SYSTEM)
        {
            LogMessage logMessage = new LogMessage(logMsg, logType);
            var lastMessageOfSameKind =
                Storage.LogMessages.FirstOrDefault(x => x.Value.TimeSent.AddSeconds(1) > DateTime.Now && x.Value.Key == logMsg);
            
            if (lastMessageOfSameKind.Value != null)
            {
                return;
            }
            if (Storage.LogMessages.TryAdd(Storage.LogMessages.Count, logMessage))
            {
                Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Id), "0|A|STM|" + logMsg + "");
            }
        }

        public void LevelChecker()
        {
            if (!World.StorageManager.Levels.PlayerLevels.ContainsKey(Information.Level.Id + 1))
                return;

            foreach (var level in World.StorageManager.Levels.PlayerLevels)
            {
                if (Information.Experience.Get() > level.Value.Experience && level.Key > Information.Level.Id)
                    LevelUp();
            }
        }

        public void LevelUp()
        {
            if (!World.StorageManager.Levels.PlayerLevels.ContainsKey(Information.Level.Id + 1))
                return;
            var lvl = World.StorageManager.Levels.PlayerLevels[Information.Level.Id + 1];
            if (Information.Experience.Get() < lvl.Experience)
                Information.Experience.Add(lvl.Experience - Information.Experience.Get() + 1);

            Information.LevelUp(lvl);
            var gameSession = World.StorageManager.GetGameSession(Id);
            Packet.Builder.LevelUpCommand(gameSession);
            Refresh();
        }

        public string BuildExtrasPacket()
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

            foreach (var item in Extras)
            {
                if (item.Value.Amount > 0)
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

            return Convert.ToInt32(droneRep) + "|1|" + Convert.ToInt32(jumpToBase) + "|" +
                   Convert.ToInt32(ammoBuy) + "|" + Convert.ToInt32(rep) + "|" + Convert.ToInt32(tradeDrone) +
                   "|0|" + 1 + "|" + 1 + "|0|" + Convert.ToInt32(aim) + "|" +
                   Convert.ToInt32(autoRocket) + "|" + Convert.ToInt32(cloak) + "|" +
                   Convert.ToInt32(autoRocketLauncer) + "|" + Convert.ToInt32(rocketBuy) + "|" +
                   Convert.ToInt32(jump) + "|" + Convert.ToInt32(petRefuel);
        }

        public void LoadExtras()
        {
            Controller.CPUs.LoadCpus();
            UpdateExtras();
        }

        public void UpdateExtras()
        {
            Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Id), "0|A|ITM|" + BuildExtrasPacket());
            foreach (var type in Enum.GetValues(typeof(CPU.Types))) Controller.CPUs.Update((CPU.Types)type);
        }

        private void CharacterEnteredRange(object s, CharacterArgs e)
        {
            var charAsPlayer = e.Character as Player;
            if (charAsPlayer != null)
            {
                //if (charAsPlayer.Boosters.Count > 0)
                Booster.CalculateTotalBoost(charAsPlayer);
            }
        }

        private void CharacterExitedRange(object s, CharacterArgs e)
        {
            var charAsPlayer = e.Character as Player;
            if (charAsPlayer != null)
            {
                //if (charAsPlayer.Boosters.Count > 0)
                Booster.CalculateTotalBoost(charAsPlayer);
            }
        }

        private void TickBoosters()
        {
            foreach (var booster in Boosters)
            {
                booster.Tick();
            }
            CheckForBoosters();
        }

        private DateTime LastTimeCheckedBoosters = new DateTime();
        private void CheckForBoosters()
        {
            if (LastTimeCheckedBoosters.AddMilliseconds(5000) < DateTime.Now)
            {
                // TODO: Get boosters from mysql
                Booster.CalculateTotalBoost(this);
                LastTimeCheckedBoosters = DateTime.Now;
            }
        }

        public void BoostDamage(double value)
        {
            if (BoostedDamage < 0.5)
            {
                if (BoostedDamage + value > 0.5)
                {
                    value = 0.5 - BoostedDamage;
                }
                BoostedDamage += value;
            }
        }

        public void BoostSpeed(double value)
        {
            if (BoostedAcceleration < 2)
            {
                if (BoostedAcceleration + value > 2)
                {
                    value = 2 - BoostedAcceleration;
                }
                BoostedAcceleration += value;
                UpdateSpeed();
            }
        }

        public void UpdateSpeed()
        {
            Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Id), "0|A|v|" + Speed);
        }

        public int EnemyWarningLevel = 0;
        public void AssembleEnemyWarn()
        {
            if (GetGameSession() == null) return;
            if (Spacemap != null && State.IsOnHomeMap())
            {
                var count = Spacemap.Entities.Count(
                    x => x.Value.FactionId != FactionId && x.Value.FactionId != Faction.NONE);
                if (EnemyWarningLevel != count)
                    Packet.Builder.LegacyModule(GetGameSession(),
                        "0|n|w|" + count); //enemy warning
                EnemyWarningLevel = count;
            }
        }

        public GameSession GetGameSession()
        {
            return World.StorageManager.GetGameSession(Id);
        }
    }
}
