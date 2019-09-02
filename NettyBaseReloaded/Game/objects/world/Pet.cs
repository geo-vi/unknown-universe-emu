using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.pets;
using NettyBaseReloaded.Game.objects.world.pets.gears;
using Newtonsoft.Json.Serialization;

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

        // INFOS //
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
                var value = Hangar.Configurations[CurrentConfig - 1].TotalDamageCalculated;
                return value;
            }
        }

        public sealed override int CurrentShield
        {
            get => Hangar.Configurations[CurrentConfig - 1].CurrentShieldLeft;
            set => Hangar.Configurations[CurrentConfig - 1].CurrentShieldLeft = value;
        }

        public override int MaxShield => Hangar.Configurations[CurrentConfig - 1].TotalShieldCalculated;

        public override double ShieldAbsorption => Hangar.Configurations[CurrentConfig - 1].ShieldAbsorb;

        public int Fuel { get; set; }

        public int MaxFuel => 50000;

        public double Experience { get; set; }

        public Level Level { get; set; }

        public short ExpansionStage => 0;

        public int CurrentConfig => GetOwner().CurrentConfig;

        public override Reward Reward => Hangar.Ship.Reward;

        // GEARS //
        public Dictionary<GearType, PetGear> PetGears = new Dictionary<GearType, PetGear>();

        public PetGear ActiveGear;
        
        public Pet(int id,Player owner, string name, Hangar hangar, Faction factionId,
            Level level, double experience, int fuel) : base(id + 2000000, name, hangar, factionId)
        {
            DbId = id;
            OwnerId = owner.Id;
            Level = level;
            Experience = experience;
            Fuel = fuel;
            Clan = owner.Clan;
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
                Invalidate();
                return null;
            }
            return ownerSession.Player;
        }

        public override void Tick()
        {
            GetOwner();
            if (!Controller.Active || EntityState == EntityStates.DEAD) return;
            base.Tick();
            FuelReduction();
            LevelChecker();
        }

        private DateTime _lastTimeSynced;
        private void FuelReduction()
        {
            if (_lastTimeSynced.AddSeconds(5) > DateTime.Now) return;

            //if (Moving) Fuel -= 10;
            //Fuel -= 5;
            //if (Fuel < 0) Fuel = 0;
            //Packet.Builder.PetFuelUpdateCommand(GetOwner().GetGameSession(), this);
            _lastTimeSynced = DateTime.Now;
        }

        public void BasicSave()
        {
            World.DatabaseManager.SavePet(this);
        }

        public bool HasFuel()
        {
            return Fuel > 0;
        }

        /// <summary>
        /// Checking levels, appends to LastLevelCheck and controls for not too many checks (overloads CPU)
        /// </summary>
        private DateTime _lastLevelCheck;
        private void LevelChecker()
        {
            if (_lastLevelCheck.AddSeconds(1) > DateTime.Now) return;

            var determined = World.StorageManager.Levels.DeterminePetLvl(Experience);
            if (determined != null && Level != determined)
            {
                LevelUp(determined);
            }

            _lastLevelCheck = DateTime.Now;
        }

        /// <summary>
        /// Levels up the PET
        /// </summary>
        /// <param name="targetLevel"></param>
        private void LevelUp(Level targetLevel)
        {
            Level = targetLevel;
            BasicSave();
            Hangar.Ship = GetShipByLevel(targetLevel.Id);
            var gameSession = GetOwner().GetGameSession();
            Packet.Builder.PetLevelUpdateCommand(gameSession, this);
            Packet.Builder.PetStatusCommand(gameSession, this);
        }

        /// <summary>
        /// Same as the other Destroy method
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            Controller.OnPetDestruction();
        }

        /// <summary>
        /// After Pet got destroyed this method gets called and should disable pet
        /// </summary>
        /// <param name="destroyer"></param>
        public override void Destroy(Character destroyer)
        {
            base.Destroy(destroyer);
            Controller.OnPetDestruction();
        }

        private void RefreshPetInitializationWindow()
        {
            var owner = GetOwner();
            if (owner == null) return;
            Packet.Builder.PetInitializationCommand(owner.GetGameSession(), this);
        }

        /// <summary>
        /// Gets the pet level's ship
        /// </summary>
        /// <param name="level">level id</param>
        /// <returns></returns>
        public static Ship GetShipByLevel(int level)
        {
            switch(level)
            {
                case 3:
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

        /// <summary>
        /// Will stop the pet
        /// </summary>
        public override void Invalidate()
        {
            base.Invalidate();
            Controller.Exit();
        }

        private Level GetNextLevel()
        {
            var levels = World.StorageManager.Levels.PetLevels;
            if (levels.ContainsKey(Level.Id + 1)) return levels[Level.Id + 1];
            return Level;
        }
        
        public double GetMaxExp()
        {
            return Level.Experience;
        }

        public void RefreshConfig()
        {
            PetGears.Clear();
            PetGears.Add(GearType.PASSIVE, new PetPassiveGear(this));
            PetGears.Add(GearType.GUARD, new PetGuardGear(this));
            
            foreach (var item in Hangar.Configurations[CurrentConfig - 1].EquippedItemsOnShip)
            {
                switch (item.Value.Item.LootId)
                {
                    case "pet_gear_g-kk1": // Kamikaze
                        PetGears.Add(GearType.KAMIKAZE, new PetKamikazeGear(this, item.Value.Level));
                        break;
                    case "pet_gear_g-al1": // Auto Loot
                        PetGears.Add(GearType.AUTO_LOOT, new PetCollectorGear(this, item.Value.Level));
                        break;
                    case "pet_gear_g-ar1": // Resource collector
                        PetGears.Add(GearType.AUTO_RESOURCE_COLLECTION, new PetResourceCollectorGear(this, item.Value.Level));
                        break;
                    case "pet_gear_g-el1":
                        PetGears.Add(GearType.ENEMY_LOCATOR, new PetEnemyLocatorGear(this, item.Value.Level));
                        break;
                    case "pet_gear_g-rep1":
                        PetGears.Add(GearType.REPAIR_PET, new PetRepairGear(this, item.Value.Level));
                        break;
                    case "pet_gear_g-rl1":
                        PetGears.Add(GearType.RESOURCE_LOCATOR, new PetResourceLocatorGear(this, item.Value.Level));
                        break;
                    case "pet_gear_g-tra1":
                        PetGears.Add(GearType.TRADE_POD, new PetTradePodGear(this, item.Value.Level));
                        break;
                    case "pet_gear_cgm-02":
                        PetGears.Add(GearType.COMBO_GUARD, new PetComboGuardGear(this, item.Value.Level));
                        break;
                    case "pet_gear_csr-02":
                        PetGears.Add(GearType.COMBO_SHIP_REPAIR, new PetComboShipRepairGear(this, item.Value.Level));
                        break;
                }
            }

            Controller.SwitchGear(GearType.PASSIVE, 0);
        }
    }
}
