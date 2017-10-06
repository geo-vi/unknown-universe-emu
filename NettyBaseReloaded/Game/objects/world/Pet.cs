using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players;
using Gear = NettyBaseReloaded.Game.controllers.pet.Gear;

namespace NettyBaseReloaded.Game.objects.world
{
    class Pet : Character
    {
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

        public override int MaxHealth => Hangar.Ship.Health;

        public int Fuel { get; set; }

        public int Experience { get; set; }

        public Level Level { get; set; }

        public List<Gear> Gears { get; set; }

        public Pet(int id,int ownerId, string name, Hangar hangar, int currentHealth, int currentNano, Faction factionId,
            Level level, int experience, int fuel, List<Gear> gears) : base(id + 2000000, name, hangar, factionId, hangar.Position, hangar.Spacemap,
            new Reward(0,0), new CargoDrop())
        {
            OwnerId = ownerId;
            CurrentHealth = currentHealth;
            CurrentNanoHull = currentNano;
            Level = level;
            Experience = experience;
            Fuel = fuel;
            Gears = gears;
        }

        /// <summary>
        /// This should get the Player class of the owner
        /// </summary>
        /// <returns>Returning Player class of pet's owner</returns>
        public Player GetOwner()
        {
            return World.StorageManager.GameSessions[OwnerId].Player;
        }

        public void Tick()
        {
            FuelReduction();
        }

        public DateTime LastTimeSynced = new DateTime(2017, 2, 5, 0, 0, 0);
        public void FuelReduction()
        {
            if (LastTimeSynced.AddSeconds(1) > DateTime.Now) return;

            if (Moving) Fuel -= 2;
            Fuel -= 1;
        }

        public void BasicSave()
        {
            
        }

        public bool HasFuel()
        {
            return Fuel > 0;
        }
    }
}
