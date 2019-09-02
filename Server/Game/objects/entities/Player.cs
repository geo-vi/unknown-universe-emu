using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Server.Game.controllers;
using Server.Game.objects.entities.players;
using Server.Game.objects.entities.ships;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;
using Server.Game.objects.maps.objects;
using Server.Game.objects.maps.objects.assets;
using Server.Game.objects.maps.objects.assets.triggered;
using Server.Game.objects.server;
using Server.Main;
using Server.Main.objects;

namespace Server.Game.objects.entities
{
    class Player : Character
    {
        /**********
         * BASICS *
         **********/

        #region Basics Variables
        public int GlobalId;

        public string SessionId { get; set; }

        public Ranks RankId { get; set; }
        
        public new PlayerController Controller { get; set; }

        public override Reward Reward => Hangar.Ship.Reward;

        #endregion
        /***************
         * INFORMATION *
         ***************/

        public override Hangar Hangar
        {
            get { return Equipment.GetActiveHangar(); }
        }

        public Equipment Equipment { get; set; }
         
         public Ammunition Ammunition { get; set; }
         
         public Information Information { get; set; }
         
         public Cargo Cargo { get; set; }
         
         public PlayerGates Gates { get; set; }
         
         public PlayerSettings Settings { get; set; }
         
        /*********
         * EXTRA *
         *********/

        public Pet Pet { get; set; }
        
        public Group Group { get; set; }
        
        /*********
         * STATS *
         *********/
        
        private double BoostedHealth { get; set; }
        public override int MaxHealth
        {
            get
            {
                var value = Hangar.Ship.Health; 
                return value;
            }
        }

        private double BoostedShield { get; set; }
        public override int MaxShield
        {
            get
            {
                var value = GetCurrentConfiguration().TotalShieldCalculated;
                return value;
            }
        }

        public override int CurrentShield
        {
            get
            {
                var value = GetCurrentConfiguration().CurrentShieldLeft;
                return value;
            }
            set { Hangar.Configurations[CurrentConfig - 1].CurrentShieldLeft = value; }
        }

        public override double ShieldAbsorption
        {
            get
            {
                var value = GetCurrentConfiguration().ShieldAbsorb;
                return value;
            }
        }

        public override double ShieldPenetration
        {
            get { return 0; }
        }

        public double BoostedAcceleration { get; set; }
        public override int Speed
        {
            get
            {
                var value = Hangar.Ship.Speed;
                value += GetCurrentConfiguration().TotalSpeedCalculated;
                return value;
            }
        }

        public double BoostedDamage { get; set; }
        public override int Damage
        {
            get
            {
                var value = GetCurrentConfiguration().TotalDamageCalculated;
                return value;
            }
        }

        public override int RocketDamage
        {
            get
            {
                var value = 1000;
                return value;
            }
        }

        public Dictionary<int, Extra> Extras
        {
            //get { return Hangar.Configurations[CurrentConfig - 1].Extras; }
            get { return null; }
        }

        public override RocketLauncher RocketLauncher
        {
            get
            {
                return GetCurrentConfiguration().RocketLauncher;
            }
        }

        public override int AttackRange => 800;

        public int MapWarningLevel { get; set; }

        /// <summary>
        /// This is a for the multi-client support.
        /// - Work in progress -
        /// </summary>
        public bool UsingNewClient { get; set; }
        
        public Player(int id, int globalId, string name, Clan clan, Factions factionId, string sessionId, Ranks rankId, bool usingNewClient = false) : base(id, name, null, factionId, clan)
        {
            GlobalId = globalId;
            SessionId = sessionId;
            RankId = rankId;
            UsingNewClient = usingNewClient;
            CurrentConfig = 1;
        }

        public Configuration GetCurrentConfiguration()
        {
            var config = Hangar.Configurations[CurrentConfig - 1];
            return config;
        }

        public bool IsMapIntruder()
        {
            //Get from statistics
            return false;
        }
    }
}
