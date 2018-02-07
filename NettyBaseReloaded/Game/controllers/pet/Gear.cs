using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.controllers.pet
{
    abstract class Gear : IChecker
    {
        public PetController baseController { get; }

        public GearType Type { get; internal set; }

        public int Level { get; set; }

        public int Amount { get; set; }

        public bool Enabled { get; set; }

        protected Gear(PetController controller, bool enabled, int level, int amount = 1)
        {
            baseController = controller;
            Enabled = enabled;
            Level = level;
            Amount = amount;
            controller.Shutdown += (sender, args) => End();
        }

        public abstract void Activate();

        public abstract void Check();

        public abstract void End();

        public void Follow(Character character)
        {
            var pet = baseController.Pet;
            var distance = pet.Position.DistanceTo(character.Position);
            if (distance < 200 && character.Moving) return;
            
            if (character.Moving)
            {
                MovementController.Move(pet, character.Position);
            }
            else if (Math.Abs(distance - 300) > 250 && !pet.Moving)
                MovementController.Move(pet, Vector.GetPosOnCircle(character.Position, 300));
        }
    }
}
