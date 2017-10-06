using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.pet
{
    abstract class Gear : IChecker
    {
        public PetController baseController { get; }

        public Gear(PetController controller)
        {
            baseController = controller;
        }

        public abstract void Activate();

        public abstract void Check();

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
