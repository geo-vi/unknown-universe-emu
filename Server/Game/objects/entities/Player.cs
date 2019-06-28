using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Server.Game.controllers;
using Server.Game.objects.entities.players;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;
using Server.Game.objects.maps.objects;
using Server.Game.objects.maps.objects.assets;
using Server.Game.objects.maps.objects.assets.triggered;
using Server.Main;
using Server.Main.objects;

namespace Server.Game.objects.entities
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

        public Ranks RankId { get; set; }
        
        public override Reward Reward => Hangar.Ship.Reward;

        #endregion
        /***************
         * INFORMATION *
         ***************/

        #region Information Variables
//        public Equipment Equipment { get; private set; }
//
//        public Statistics Statistics { get; private set; }
//
//        public Information Information { get; private set; }
//
//        public State State { get; private set; }

        public override Hangar Hangar { get; set; }
//public override Hangar Hangar => Equipment.ActiveHangar;
        #endregion
        
        
        /*********
         * EXTRA *
         *********/

        public Pet Pet { get; set; }

//        public Settings Settings { get; private set; }
//
//        public Storage Storage { get; private set; }

        public ConcurrentDictionary<int, Booster> Boosters { get; set; }

//        public ConcurrentDictionary<Abilities, Ability> Abilities { get; set; }

        public ConcurrentDictionary<Player, Booster> InheritedBoosters = new ConcurrentDictionary<Player, Booster>();

        public Group Group { get; set; }

        public ConcurrentDictionary<Techs, Tech> Techs = new ConcurrentDictionary<Techs, Tech>();

//        public PlayerGates Gates { get; set; }
//
//        public Skylab Skylab { get; set; }
//
//        public QuestPlayerData QuestData;
//
//        public Announcements Announcements;

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
//                if (Hangar.Drones.Count(x => x.Value.GetDroneDesign() == 2) == Hangar.Drones.Count)
//                {
//                    value = (int)(value * 1.2);
//                }
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
                //value = (int)(value * Hangar.Ship.GetHealthBonus(this));
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
//                value += (int)(Hangar.Drones.Count(x => x.Value.GetDroneDesign() == 2) * 0.2 * value);
//                switch (Formation)
//                {
//                    case DroneFormation.HEART:
//                    case DroneFormation.TURTLE:
//                        value = (int)(value * 1.1); //+10%
//                        break;
//                    case DroneFormation.DOUBLE_ARROW:
//                        value = (int)(value * 0.8); //-20%
//                        break;
//                }
//                value = (int)(value * Hangar.Ship.GetShieldBonus(this));
//                value = (int) (value * (BoostedShield + 1));
//                value += (int)(value * Skylab.GetShieldBonus());

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

                //value += (int)(value * Skylab.GetSpeedBonus());

                if (BoostedAcceleration > 0)
                    value = (int)(value * (1 + BoostedAcceleration));
                //if (Controller.Effects.SlowedDown) value = (int)(value * 0.5);
                
                return value;
            }
        }

        public double BoostedDamage;
        public override int Damage
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].TotalDamageCalculated;
                //if (Hangar.Drones.All(x => x.Value.GetDroneDesign() == 1)) value = (int)(value * 1.1);
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

                //value += (int)(value * Skylab.GetLaserDamageBonus());
//                value = (int) (value * Hangar.Ship.GetDamageBonus(this));
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

                //value += (int)(value * Skylab.GetRocketDamageBonus());

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
            //get { return Hangar.Configurations[CurrentConfig - 1].Extras; }
            get { return null; }
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

        //public ConcurrentDictionary<int, PlayerEvent> EventsPraticipating = new ConcurrentDictionary<int, PlayerEvent>();

        public ConcurrentDictionary<int, GalaxyGate> OwnedGates = new ConcurrentDictionary<int, GalaxyGate>();

        private bool Unloaded;

//        public bool IsLoaded => Settings != null && Equipment != null && Storage != null && Controller != null &&
//                                !Controller.StopController && !Unloaded && State != null && !State.Jumping;
//
//        // ** THREAD LOCKER ** //

        public int MapWarningLevel;

        public Player(int id, int globalId, string name, Clan clan, Factions factionId, string sessionId, Ranks rankId, bool usingNewClient = false) : base(id, name, null, factionId, clan)
        {
            Init();
            GlobalId = globalId;
            SessionId = sessionId;
            RankId = rankId;
            UsingNewClient = usingNewClient;
            CurrentConfig = 1;
        }

        public void Init()
        {

        }
    }
}
