using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.controllers.login;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.extra;
using NettyBaseReloaded.Game.objects.world.players.extra.abilities;
using NettyBaseReloaded.Game.objects.world.players.quests;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;
using State = NettyBaseReloaded.Game.objects.world.players.State;

namespace NettyBaseReloaded.Game.objects.world
{
    class Player : Character
    {
        /// <summary>
        /// TODO: RECODE the PlayerBase class for a bit more order
        /// Ex: Tick all playerbased classes at once instead each one individually @Tick() method.
        /// </summary>

        /**********
         * BASICS *
         **********/

        #region Basics Variables
        public int GlobalId;

        public string SessionId { get; set; }

        public Rank RankId { get; set; }
        public new PlayerController Controller { get; set; }

        public override Reward Reward => Hangar.Ship.Reward;

        #endregion
        /***************
         * INFORMATION *
         ***************/

        #region Information Variables
        public Equipment Equipment { get; private set; }

        public Statistics Statistics { get; private set; }

        public Information Information { get; private set; }

        public State State { get; private set; }

        public override Hangar Hangar => Equipment.ActiveHangar;
        #endregion
        /*********
         * EXTRA *
         *********/

        public Pet Pet { get; set; }

        public Settings Settings { get; private set; }

        public Storage Storage { get; private set; }

        public ConcurrentDictionary<int, Booster> Boosters { get; set; }

        public ConcurrentDictionary<Abilities, Ability> Abilities { get; set; }

        public ConcurrentDictionary<Player, Booster> InheritedBoosters = new ConcurrentDictionary<Player, Booster>();

        public Group Group { get; set; }

        public ConcurrentDictionary<Techs, Tech> Techs = new ConcurrentDictionary<Techs, Tech>();

        public PlayerGates Gates { get; set; }

        public Skylab Skylab { get; set; }

        public QuestPlayerData QuestData;

        public Announcements Announcements;

        /*********
         * STATS *
         *********/

        public int CurrentConfig { get; set; }

        public double BoostedHealth;
        public override int MaxHealth
        {
            get
            {
                var value = Hangar.Ship.Health;
                if (Hangar.Drones.Count(x => x.Value.GetDroneDesign() == 2) == Hangar.Drones.Count)
                {
                    value = (int)(value * 1.2);
                }
                switch (Formation)
                {
                    case DroneFormation.CHEVRON:
                        value = (int)(value * 0.8); // -20%
                        break;
                    case DroneFormation.DIAMOND:
                        value = (int)(value * 0.7); //-30%
                        break;
                    case DroneFormation.MOTH:
                    case DroneFormation.HEART:
                        value = (int)(value * 1.2); // +20%
                        break;
                }
                value = (int)(value * Hangar.Ship.GetHealthBonus(this));
                value = (int) (value * (BoostedHealth + 1));
                return value;
            }
        }

        public double BoostedShield;
        public override int MaxShield
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].TotalShieldCalculated;
                value += (int)(Hangar.Drones.Count(x => x.Value.GetDroneDesign() == 2) * 0.2 * value);
                switch (Formation)
                {
                    case DroneFormation.HEART:
                    case DroneFormation.TURTLE:
                        value = (int)(value * 1.1); //+10%
                        break;
                    case DroneFormation.DOUBLE_ARROW:
                        value = (int)(value * 0.8); //-20%
                        break;
                }
                value = (int)(value * Hangar.Ship.GetShieldBonus(this));
                value = (int) (value * (BoostedShield + 1));
                value += (int)(value * Skylab.GetShieldBonus());

                return value;
            }
        }

        public override int CurrentShield
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].CurrentShieldLeft;
                return value;
            }
            set { Hangar.Configurations[CurrentConfig - 1].CurrentShieldLeft = value; }
        }

        public override double ShieldAbsorption
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].ShieldAbsorb;
                switch (Formation)
                {
                    case DroneFormation.CRAB:
                        value += 0.4;
                        break;
                    case DroneFormation.BARRAGE:
                        value -= 0.15;
                        break;
                }

                if (value > 1) value = 1;

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

        public double BoostedAcceleration;
        public override int Speed
        {
            get
            {
                var value = Hangar.Ship.Speed;
                value += Hangar.Configurations[CurrentConfig - 1].TotalSpeedCalculated;
                switch (Formation)
                {
                    case DroneFormation.BAT:
                        value = (int)(value * 0.85);
                        break;
                }

                value += (int)(value * Skylab.GetSpeedBonus());

                if (BoostedAcceleration > 0)
                    value = (int)(value * (1 + BoostedAcceleration));
                if (Controller.Effects.SlowedDown) value = (int)(value * 0.5);
                
                return value;
            }
        }

        public double BoostedDamage;
        public override int Damage
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].TotalDamageCalculated;
                if (Hangar.Drones.All(x => x.Value.GetDroneDesign() == 1)) value = (int)(value * 1.1);
                switch (Formation)
                {
                    case DroneFormation.TURTLE:
                        value = (int)(value * 0.925); //-7.5%
                        break;
                    case DroneFormation.ARROW:
                        value = (int)(value * 0.97); //-3%
                        break;
                    case DroneFormation.PINCER:
                        value = (int)(value * 1.03); //+3%
                        break;
                    case DroneFormation.HEART:
                        value = (int)(value * 0.95); //-5%
                        break;
                    case DroneFormation.BARRAGE:
                        if (Selected is Npc)
                            value = (int)(value * 1.05); //+5%
                        break;
                    case DroneFormation.BAT:
                        if (Selected is Npc)
                            value = (int)(value * 1.08); //+8%
                        break;
                }

                value += (int)(value * Skylab.GetLaserDamageBonus());
                value = (int) (value * Hangar.Ship.GetDamageBonus(this));
                value = (int) (value * (BoostedDamage + 1));
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
                        value = (int)(value * 0.925); //-7.5%
                        break;
                    case DroneFormation.ARROW:
                        value = (int)(value * 1.2); //+20%
                        break;
                    case DroneFormation.STAR:
                        value = (int)(value * 1.25); //+25%
                        break;
                    case DroneFormation.CHEVRON:
                        value = (int)(value * 1.5); //+50%
                        break;
                }

                value += (int)(value * Skylab.GetRocketDamageBonus());

                return value;
            }
        }

        /// <summary>
        /// Booster related
        /// </summary>
        public double BoostedBoxRewards;

        public double BoostedHonorReward;

        public double BoostedExpReward;

        public double BoostedQuestReward;

        public double BoostedResources;

        public double BoostedRepairSpeed;

        public double BoostedShieldRegen;

        //Booster related stuff -end

        public Dictionary<int, Extra> Extras
        {
            get { return Hangar.Configurations[CurrentConfig - 1].Extras; }
        }

        public override RocketLauncher RocketLauncher
        {
            get { return Hangar.Configurations[CurrentConfig - 1].RocketLauncher; }
        }

        public override int AttackRange => 800;

        /// <summary>
        /// This is a for the multi-client support.
        /// - Work in progress -
        /// </summary>
        public bool UsingNewClient { get; set; }

        public List<Npc> AttachedNpcs = new List<Npc>();

        public ConcurrentDictionary<int, PlayerEvent> EventsPraticipating = new ConcurrentDictionary<int, PlayerEvent>();

        public ConcurrentDictionary<int, GalaxyGate> OwnedGates = new ConcurrentDictionary<int, GalaxyGate>();

        private bool Unloaded;

        public bool IsLoaded => Settings != null && Equipment != null && Storage != null && Controller != null &&
                                !Controller.StopController && !Unloaded && State != null && !State.Jumping;

        // ** THREAD LOCKER ** //
        private readonly object ThreadLocker = new object();

        public Player(int id, int globalId, string name, Clan clan, Faction factionId, string sessionId, Rank rankId, bool usingNewClient = false) : base(id, name, null, factionId, clan)
        {
            InitializeClasses();
            GlobalId = globalId;
            SessionId = sessionId;
            RankId = rankId;
            UsingNewClient = usingNewClient;
            CurrentConfig = 1;
        }

        private void InitializeClasses()
        {
            try
            {
                Equipment = new Equipment(this);
                Statistics = World.DatabaseManager.LoadStatistics(this);
                Information = new Information(this);
                State = new State(this);
                Storage = new Storage(this);
                Boosters = World.DatabaseManager.LoadBoosters(this);
                Abilities = Hangar.Ship.Abilities(this);
                Settings = new Settings(this);
                Skylab = World.DatabaseManager.LoadSkylab(this);
                Pet = World.DatabaseManager.LoadPet(this);
                QuestData = new QuestPlayerData(this);
                Announcements = new Announcements(this);
                Gates = new PlayerGates(this);
                World.DatabaseManager.SavePlayerHangar(this, Hangar);
            }
            catch (Exception exception)
            {
                // getting rid of user
                Console.WriteLine("Exception found.. Disconnecting user");
                Console.WriteLine("Exception: " + exception + ";" + exception.StackTrace + ";" + exception.Message);

                var session = GetGameSession();
                if (session != null)
                {
                    session.Kick();
                }
                else Invalidate();
            }
        }

        public override void Tick()
        {
            lock (ThreadLocker)
            {
                if (GetGameSession() == null)
                {
                    Invalidate();
                    return;
                }

                if (!Controller.Active || EntityState == EntityStates.DEAD)
                    return;

                base.Tick();

                LevelChecker();
                TickBoosters();
                AssembleEnemyWarn();
                TickEvents();
                TickTechs();
                TickAbilities();
                TickQuests();
                TickAnnouncements();
                Skylab.Tick();
                Gates.Tick();
                State.Tick();
                Information.Tick();
            }
        }


        public override void Invalidate()
        {
            lock (ThreadLocker)
            {
                Save();
                Unloaded = true;
                base.Invalidate();
                Pet?.Invalidate();
                Controller?.Exit();
                Storage.Clean();
                State.Reset();
            }
        }

        private void TickTechs()
        {
            foreach (var tech in Techs.Values) { tech.Tick(); }
        }

        private void TickEvents()
        {
            foreach (var gameEvent in EventsPraticipating) { gameEvent.Value.Tick(); }
        }

        private void TickAbilities()
        {
            foreach (var ability in Abilities.Values) { ability.Tick(); }
        }

        private void TickQuests()
        {
            QuestData?.Tick();
        }

        private void TickAnnouncements()
        {
            Announcements.Tick();
        }

        public void ClickableCheck(Object obj)
        {
            if (obj is IClickable && obj.Position != null)
            {
                var active = Vector.IsInRange(Position, obj.Position, obj.Range);
                Packet.Builder.MapAssetActionAvailableCommand(World.StorageManager.GetGameSession(Id), obj, active);
            }
        }

        public void LoadObject(Object obj)
        {
            if (obj == null || obj.Position == null || GetGameSession() == null)
            {
                return;
            }

            if (obj is Station) Storage.LoadStation(obj as Station);
            else if (obj is Jumpgate) Storage.LoadPortal(obj as Jumpgate);
            else if (obj is Asteroid) Storage.LoadAsteroid(obj as Asteroid);
            else if (obj is Asset) Storage.LoadAsset(obj as Asset);
            else if (obj is Billboard) Storage.LoadBillboard(obj as Billboard);
            else
            {
                //if (obj.Position.DistanceTo(Position) < 2000)
                //{
                    if (obj is Collectable) Storage.LoadCollectable(obj as Collectable);
                    else if (obj is Ore) Storage.LoadResource(obj as Ore);
                    else if (obj is Mine) Storage.LoadMine(obj as Mine);
                    else if (obj is Firework) Storage.LoadFirework(obj as Firework);
                //}
                if (!Storage.LoadedObjects.ContainsKey(obj.Id))
                    Storage.LoadedObjects.TryAdd(obj.Id, obj);
            }
        }

        public void UnloadObject(Object obj)
        {
            if (obj == null || GetGameSession() == null) return;
            switch (obj)
            {
                case Collectable collectable:
                    Storage.UnLoadCollectable(collectable);
                    break;
                case Asset asset:
                    Storage.UnloadAsset(asset);
                    break;
                case Jumpgate portal:
                    Storage.UnloadPortal(portal);
                    break;
                default:
                {
                    if (Storage.LoadedObjects.ContainsKey(obj.Id))
                        Storage.LoadedObjects.TryRemove(obj.Id, out _);
                    break;
                }
            }
        }

        private readonly DateTime LastConfigSave = new DateTime();
        public void SaveConfig()
        {
            if (LastConfigSave.AddSeconds(5) < DateTime.Now)
                World.DatabaseManager.SaveConfig(this);
        }

        public void Save()
        {
            World.DatabaseManager.SavePlayerHangar(this, Hangar);
            World.DatabaseManager.SaveConfig(this);
        }

        public void Refresh()
        {
            var gameSession = GetGameSession();
            if (gameSession == null) return;
            UpdateShip();
            ILogin.SendLegacy(gameSession);
            Updaters.Update();
            Cooldowns.SendAll();
        }

        public override void SetPosition(Vector targetPosition)
        {
            Pet?.Controller.Deactivate();
            ChangePosition(targetPosition);
            Packet.Builder.MoveCommand(GetGameSession(), this, 0);
        }

        public void ChangePosition(Vector targetPosition)
        {
            Position = targetPosition;
            OldPosition = targetPosition;
            Destination = targetPosition;
            Direction = targetPosition;
            MovementStartTime = DateTime.Now;
            MovementTime = 0;
            Moving = false;
        }

        public Tuple<Vector, Spacemap> GetClosestStation(bool isLower = false)
        {
            Spacemap map = null;
            if (Properties.Game.PVP_MODE)
            {
                map = World.StorageManager.Spacemaps[16];
            }
            else
            {
                if (Spacemap?.Id > 16 && Spacemap.Id <= 29 && !isLower && Information.Level.Id > 11)
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

            if (map != null)
            {
                var stations = map.Objects.Values.Where(x => x is Station);
                foreach (var station in stations)
                {
                    var pStation = station as Station;
                    if (pStation?.Faction == FactionId)
                    {
                        return new Tuple<Vector, Spacemap>(pStation?.Position, map);
                    }
                }
            }

            return null;
        }

        public void SendLogMessage(string logMsg, LogMessage.LogType logType = LogMessage.LogType.SYSTEM)
        {
            var logMessage = new LogMessage(logMsg, logType);
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

        private DateTime LastLevelCheck;
        public void LevelChecker()
        {
            if (LastLevelCheck.AddSeconds(1) > DateTime.Now) return;

            var determined = World.StorageManager.Levels.DeterminatePlayerLvl(Information.Experience.Get());
            if (determined != null && Information.Level != determined)
            {
                LevelUp(determined);
            }

            LastLevelCheck = DateTime.Now;
        }

        public void LevelUp(Level newLevel)
        {
            Information.LevelUp(newLevel);
            var gameSession = World.StorageManager.GetGameSession(Id);
            Packet.Builder.LevelUpCommand(gameSession);
            Refresh();
        }

        public void UpdateConfig()
        {
            var config = Hangar.Configurations[CurrentConfig - 1];
            config.ParseExtras();
            Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(Id), "0|A|ITM|" + Equipment.GetConsumablesPacket(), true);
            config.GetRocketLauncher(this);
            Controller.CPUs.LoadCpus();
        }

        private void TickBoosters()
        {
            foreach (var booster in Boosters)
            {
                booster.Value.Tick();
            }

            CheckForBoosters();
        }

        private DateTime LastTimeCheckedBoosters;
        private void CheckForBoosters()
        {
            if (LastTimeCheckedBoosters.AddMilliseconds(250) < DateTime.Now)
            {
                //var rangePlayers = Range.Entities.Where(x => x.Value is Player);
                //foreach (var rangePlayer in rangePlayers)
                //{
                //    var _rangePlayer = rangePlayer.Value as Player;
                //    if (_rangePlayer.Boosters.Count > 0)
                //    {
                        
                //    }
                //}
                Booster.CalculateTotalBoost(this);
                LastTimeCheckedBoosters = DateTime.Now;
            }
        }

        public void BoostDamage(double value)
        {
            if (BoostedDamage < 0.5)
            {
                if (value + BoostedDamage > 0.5)
                    BoostedDamage = 0.5;
                else BoostedDamage += value;
            }
        }

        public void BoostShield(double value)
        {
            if (BoostedShield < 1)
            {
                if (value + BoostedShield > 1)
                    BoostedShield = 1;
                else BoostedShield += value;
            }
        }

        public void BoostHealth(double value)
        {
            if (BoostedHealth < 0.5)
            {
                if (value + BoostedHealth > 0.5)
                    BoostedHealth = 0.5;
                else BoostedHealth += value;
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

        public void BoostQuestRewards(double value)
        {
            if (value + BoostedQuestReward > 1)
                BoostedQuestReward = 1;
            else BoostedQuestReward += value;
        }

        public void BoostBoxRewards(double value)
        {
            if (value + BoostedBoxRewards > 1)
                BoostedBoxRewards = 1;
            else BoostedBoxRewards += value;
        }

        public void BoostRepairSpeeds(double value)
        {
            if (value + BoostedRepairSpeed > 0.5)
                BoostedRepairSpeed = 0.5;
            else BoostedRepairSpeed += value;
        }

        public void BoostResourceCollection(double value)
        {
            if (value + BoostedResources > 1)
                BoostedResources = 1;
            else BoostedResources += value;
        }

        public void BoostExpReward(double value)
        {
            if (value + BoostedExpReward > 1)
                BoostedExpReward = 1;
            else BoostedExpReward += value;
        }

        public void BoostHonReward(double value)
        {
            if (value + BoostedHonorReward > 1)
                BoostedHonorReward = 1;
            else BoostedHonorReward += value;
        }
        
        public void UpdateSpeed()
        {
            Packet.Builder.AttributeShipSpeedUpdateCommand(World.StorageManager.GetGameSession(Id), Speed);
        }

        public int EnemyWarningLevel;
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

        public void MoveToMap(Spacemap map, Vector pos, int vwid)
        {
            lock (ThreadLocker)
            {
                Pet?.Controller.Deactivate();
                Spacemap.RemoveEntity(this);
                ResetPlayer();
                Spacemap = map;
                Position = pos;
                VirtualWorldId = vwid;
                ChangePosition(Position);
                Controller.AddToMap();
                Refresh();
            }
        }

        public void ResetPlayer()
        {
            Unloaded = true;
            Storage.UnloadAll();
            Storage.Clean();
            State.Reset();
            Range.Clean();
            Unloaded = false;
        }

        public int CreateGalaxyGate(GalaxyGate gate)
        {
            var id = 0;
            if (OwnedGates.Count > 0)
            {
                foreach (var key in OwnedGates.Keys)
                {
                    if (OwnedGates[key] == null)
                        id = key;
                    else id++;
                }

                if (OwnedGates.ContainsKey(id))
                    id++;
            }
            OwnedGates.TryAdd(id, gate);
            return id;
        }

        public void ChangeClan(Clan clan)
        {
            Clan = clan;
            Clan.Members.TryAdd(Id, new ClanMember(Id, Name));
            RefreshPlayersView();
            Packet.Builder.ClanTagChangedCommand(GetGameSession());
            var chatSession = Chat.Chat.StorageManager.GetChatSession(Id);
            if (chatSession != null)
            {
                chatSession.Player.Clan = clan;
            }
            RefreshPlayersView();
            RefreshMyView();
            ILogin.UpdateClanWindow(GetGameSession());
        }

        /// <summary>
        /// Will refresh my view
        /// all the players in range will get added and removed
        /// </summary>
        public void RefreshMyView()
        {
            var session = GetGameSession();
            foreach (var rangeCharacter in Range.Entities.Values)
            {
                Packet.Builder.ShipRemoveCommand(session, rangeCharacter);
                Packet.Builder.ShipCreateCommand(session, rangeCharacter);
            }
        }

        public void Setup()
        {
            if (!Global.TickManager.Exists(this))
            {
                var id = 0;
                Global.TickManager.Add(this, out id);
                SetTickId(id);
            }
        }

        public void RestartSessions()
        {
            Global.TickManager.Remove(this);
            Global.TickManager.Remove(Controller);
            Setup();
            Controller.Setup();
        }

        public bool IsStuck()
        {
            foreach (var poiZone in Spacemap.POIs)
            {
                if (poiZone.Value.IsVectorInShape(Position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
