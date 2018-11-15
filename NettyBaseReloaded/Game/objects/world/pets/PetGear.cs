using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.controllers;

namespace NettyBaseReloaded.Game.objects.world.pets
{
    abstract class PetGear
    {
        protected Pet Pet { get; }

        public GearType TypeOfGear { get; }

        public int Level;

        public int Amount;

        public bool Enabled;

        public List<int> OptionalParams = new List<int>();
        
        protected PetGear(Pet pet, GearType type, int level, int amount, bool enabled)
        {
            Pet = pet;
            TypeOfGear = type;
            Level = level;
            Amount = amount;
            Enabled = enabled;
        }
        
        public abstract void Tick();
        
        public abstract void SwitchTo(int optParam);

        public abstract void End();
    }
}