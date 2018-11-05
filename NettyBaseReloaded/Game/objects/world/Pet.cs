using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players;
using Gear = NettyBaseReloaded.Game.controllers.pet.Gear;

namespace NettyBaseReloaded.Game.objects.world
{
    class Pet : Character
    {
        /// <summary>
        /// DB Id since original is auto incremented by 2000000
        /// </summary>
        public int DbId { get; }

        /// <summary>
        /// Id of PET owner
        /// </summary>
        private int OwnerId { get; }

        public new PetController Controller { get; set; }

        public override int Speed
        {
            get
            {
               if (GetOwner() != null)
                    return (int)(GetOwner().Speed * 1.25);
                return 300;
            }
        }

        public override int CurrentHealth { get => Hangar.Health; set => Hangar.Health = value; }

        public sealed override int MaxHealth => Hangar.Ship.Health;

        public sealed override int Damage
        {
            get
            {
                var value = Hangar.Configurations[CurrentConfig - 1].Damage;
                return value;
            }
        }

        public sealed override int CurrentShield
        {
            get
            {
                return Hangar.Configurations[CurrentConfig - 1].CurrentShield;
            }
            set
            {
                Hangar.Configurations[CurrentConfig - 1].CurrentShield = value;
            }
        }

        public sealed override int MaxShield
        {
            get
            {
                return Hangar.Configurations[CurrentConfig - 1].MaxShield;
            }
        }

        public sealed override double ShieldAbsorption
        {
            get
            {
                return Hangar.Configurations[CurrentConfig - 1].ShieldAbsorbation;
            }
        }

        public int Fuel { get; set; }

        public int MaxFuel => 50000;

        public int Experience { get; set; }

        public Level Level { get; set; }

        public List<Gear> Gears { get; set; }

        public short DesignId
        {
            get
            {
                if (Level.Id <= 3)
                    return 12;
                if (Level.Id <= 6)
                    return 13;
                if (Level.Id <= 9)
                    return 14;
                if (Level.Id <= 12)
                    return 15;
                return 22;
            }
        }

        public short ExpansionStage => (short)Hangar.Configurations[CurrentConfig - 1].LaserCount;

        public int CurrentConfig => GetOwner().CurrentConfig;

        public Pet(int id,Player owner, string name, Hangar hangar, int currentHealth, Faction factionId,
            Level level, int experience, int fuel) : base(id + 2000000, name, hangar, factionId, hangar.Position, hangar.Spacemap,
            new Reward(0,0))
        {
            DbId = id;
            OwnerId = owner.Id;
            Level = level;
            Experience = experience;
            Fuel = fuel;
            Clan = owner.Clan;
            Gears = new List<Gear>();
            if (CurrentHealth <= 0) EntityState = EntityStates.DEAD;
        }

        /// <summary>
        /// This should get the Player class of the owner
        /// </summary>
        /// <returns>Returning Player class of pet's owner</returns>
        public Player GetOwner()
        {
            var ownerSession = World.StorageManager.GetGameSession(OwnerId);
            if (ownerSession == null)
            {
                Controller.Exit();
                return null;
            }
            return ownerSession.Player;
        }

        public override void AssembleTick(object sender, EventArgs eventArgs)
        {
            if (!Controller.Active || EntityState == EntityStates.DEAD) return;
            base.AssembleTick(sender, eventArgs);
            FuelReduction();
            LevelChecker();
        }

        public DateTime LastTimeSynced = new DateTime(2017, 2, 5, 0, 0, 0);
        public void FuelReduction()
        {
            if (LastTimeSynced.AddSeconds(5) > DateTime.Now) return;

            if (Moving) Fuel -= 10;
            Fuel -= 5;
            if (Fuel < 0) Fuel = 0;
            Packet.Builder.PetFuelUpdateCommand(GetOwner().GetGameSession(), this);
            LastTimeSynced = DateTime.Now;
        }

        public void BasicSave()
        {
            World.DatabaseManager.SavePet(this);
        }

        public bool HasFuel()
        {
            return Fuel > 0;
        }

        private DateTime LastLevelCheck = new DateTime();
        private void LevelChecker()
        {
            if (LastLevelCheck.AddSeconds(1) > DateTime.Now) return;

            var determined = World.StorageManager.Levels.DeterminatePlayerLvl(Experience);
            if (Level != determined)
            {
                LevelUp(determined);
            }

            LastLevelCheck = DateTime.Now;
        }

        public void LevelUp(Level targetLevel)
        {
            Level = targetLevel;
            BasicSave();
            var gameSession = GetOwner().GetGameSession();
            var levels = World.StorageManager.Levels.PetLevels;
            Level nextLevel = targetLevel;
            if (levels.ContainsKey(targetLevel.Id + 1)) targetLevel = levels[targetLevel.Id + 1];
            Packet.Builder.PetLevelUpdateCommand(gameSession, this, nextLevel);
        }

        public override void Destroy()
        {
            base.Destroy();
            Controller.Destroy();
        }

        public override void Destroy(Character destroyer)
        {
            base.Destroy(destroyer);
            Controller.Destroy();
        }

        public static Ship GetShipByLevel(int level)
        {
            switch(level)
            {
                case 4:
                case 5:
                case 6:
                    return World.StorageManager.Ships[13];
                case 7:
                case 8:
                case 9:
                    return World.StorageManager.Ships[14];
                case 10:
                case 11:
                case 12:
                    return World.StorageManager.Ships[15];
                case 13:
                case 14:
                case 15:
                    return World.StorageManager.Ships[22];
                default:
                    return World.StorageManager.Ships[12];
            }
        }
    }
}
