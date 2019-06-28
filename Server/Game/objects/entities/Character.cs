using System;
using Server.Game.controllers;
using Server.Game.controllers.characters;
using Server.Game.controllers.server;
using Server.Game.objects.entities.players;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Main;
using Server.Main.objects;

namespace Server.Game.objects.entities
{
    class Character : IAttackable
    {
        /**********
         * BASICS *
         **********/

        public string Name { get; set; }

        public Hangar _hangar;
        public virtual Hangar Hangar
        {
            get
            {
                if (this is Player)
                {
                    var temp = (Player) this;
                    return temp.Hangar;
                }
                return _hangar;
            }
            set
            {
                if (this is Player)
                {
                    var temp = (Player)this;
                    temp.Hangar = value;
                }
                _hangar = value;
            }
        }

        public override Factions FactionId { get; set; }

        public Clan Clan { get; set; }

        public virtual Reward Reward { get; set; }

        /************
         * POSITION *
         ************/
        public override Vector Position
        {
            get => Hangar.Position;
            set => Hangar.Position = value;
        }

        public int VirtualWorldId { get; set; }

        private Spacemap _baseSpacemap
        {
            get => Hangar.Spacemap;
            set => Hangar.Spacemap = value;
        }
        public override Spacemap Spacemap
        {
            get
            {
                var spacemap = _baseSpacemap;
                if (VirtualWorldId != 0 && spacemap.VirtualWorlds.ContainsKey(VirtualWorldId))
                    return spacemap.VirtualWorlds[VirtualWorldId];
                return spacemap;
            }
            set
            {
                if (VirtualWorldId == 0)
                    _baseSpacemap = value;
                else Spacemap.VirtualWorlds[VirtualWorldId] = value;
            }
        }

        /*********
         * STATS *
         *********/
        public override int MaxHealth { get; set; }

        public override int CurrentHealth
        {
            get { return Hangar.Health; }
            set
            {
                Hangar.Health = (value < MaxHealth) ? value : MaxHealth;
                if (value < 0) Hangar.Health = 0;
            }
        }

        public override int MaxShield { get; set; }
        public override int CurrentShield { get; set; }
        public override double ShieldAbsorption { get; set; }
        public override double ShieldPenetration { get; set; }

        //The max amount of nanohull will be the max ship health
        public override int MaxNanoHull => Hangar.Ship.Health;

        public override int CurrentNanoHull
        {
            get { return Hangar.Nanohull; }
            set
            {
                Hangar.Nanohull = (value < MaxNanoHull) ? value : MaxNanoHull;
                if (value < 0) Hangar.Nanohull = 0;
            }
        }

        public virtual int Speed
        {
            get
            {
                var value = Hangar.Ship.Speed;
                
                //if (Controller.Effects.SlowedDown) value = (int)(value * 0.1);
                
                return value;
            }
        }

        public virtual int Damage { get; set; }
        public virtual int RocketDamage { get; set; }

        /************
         * MOVEMENT *
         ************/
        public bool Moving { get; set; }
        public Vector OldPosition { get; set; }
        public Vector Destination { get; set; }
        public Vector Direction { get; set; }
        public DateTime MovementStartTime { get; set; }
        public int MovementTime { get; set; }

        /*********
         * EXTRA *
         *********/
        public IAttackable Selected { get; set; }
        public Character SelectedCharacter => Selected as Character;

//        public Range Range { get; }

        public virtual RocketLauncher RocketLauncher { get; set; }

        public DroneFormation Formation = DroneFormation.STANDARD;

//        public CooldownsAssembly Cooldowns;
//
//        public Updaters Updaters { get; set; }


        protected Character(int id, string name, Hangar hangar, Factions factionId,
            Clan clan = null) : base(id)
        {
            Name = name;
            if (hangar != null)
                Hangar = hangar;
            FactionId = factionId;
            Clan = clan;

            //Default initialization
            Moving = false;
            OldPosition = new Vector(0, 0);
            Destination = new Vector(0,0);
            Direction = new Vector(0, 0);
            MovementStartTime = new DateTime();
            MovementTime = 0;

            LastCombatTime = DateTime.Now;
            
            if (clan == null)
            {
                Clan = Global.StorageManager.Clans[0];
            }
        }

        public bool HasWarnBox(StateController controller)
        {
            if (!(this is Player)) return false;
            if (controller.IsInState(CharacterStates.HomeMap))
                return false;

            if (Spacemap.Id == 9 || Spacemap.Id == 5 || Spacemap.Id == 1)
            {
                return true;
            }

            return false;
        }
    }
}
