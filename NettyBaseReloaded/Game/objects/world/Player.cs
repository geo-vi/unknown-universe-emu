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
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;
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
            get
            {
                return Hangar.Configurations[CurrentConfig - 1].Speed;
            }
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

        /// <summary>
        /// This is a for the multi-client support.
        /// - Work in progress -
        /// </summary>
        public bool UsingNewClient { get; set; }

        /// <summary>
        /// Lists
        /// </summary>
        public List<Drone> Drones => Hangar.Drones;
        private List<LogMessage> LogMessages = new List<LogMessage>();
        public List<Npc> AttachedNpcs = new List<Npc>();

        public Player(int id, string name, Clan clan, Hangar hangar, int currentHealth, int currentNano, Faction factionId, Vector position, Spacemap spacemap, Reward rewards, CargoDrop cargoDrop, string sessionId, Rank rankId, bool usingNewClient = false) : base(id, name, hangar, factionId, position, spacemap, rewards, cargoDrop, clan)
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
        }
        
        private void InitializeClasses()
        {
            Equipment = new Equipment(this);
            Statistics = World.DatabaseManager.LoadStatistics(this);
            Information = new Information(this);
            Settings = new Settings(this);
            State = new State(this);
            Storage = new Storage(this);
            Log = new PlayerLog(SessionId);
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
            if (obj is Station) Storage.LoadStation(obj as Station);
            else if (obj is Jumpgate) Storage.LoadPortal(obj as Jumpgate);
            else if (obj is Asteroid) Storage.LoadAsteroid(obj as Asteroid);
            else if (obj is Asset) Storage.LoadAsset(obj as Asset);
            else if (obj is Collectable) Storage.LoadCollectable(obj as Collectable);
            else if (obj is Ore) Storage.LoadResource(obj as Ore);
        }

        public void Save()
        {
            Console.WriteLine("Attempt to save player");
            //World.DatabaseManager.SavePlayer(this);
        }

        public void Refresh()
        {
            Packet.Builder.ShipInitializationCommand(World.StorageManager.GetGameSession(Id));
        }

        public void SetPosition(Vector targetPosition)
        {
            if (Moving)
                MovementController.Move(this, MovementController.ActualPosition(this));
            Position = targetPosition;
            Refresh();
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
                if (Spacemap.Id < 16)
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
                else
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
            }
            var stations = map.Objects.Values.Where(x => x is Station);
            foreach (var station in stations)
            {
                var pStation = station as Station;
                if (pStation.Faction == FactionId)
                {
                    return new Tuple<Vector, Spacemap>(pStation.Position, map);
                }
            }
            return null;
        }

        public void SendLogMessage(string logMsg)
        {
            if (LogMessages.Any(x => x.TimeSent.AddSeconds(1) > DateTime.Now && x.Key == logMsg))
            {
                return;
            }
            LogMessages.Add(new LogMessage(logMsg));
            Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Id), "0|A|STM|" + logMsg + "");
        }
    }
}
