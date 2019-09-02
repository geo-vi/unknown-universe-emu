using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.controllers.pets
{
    abstract class PetSubController : AbstractedSubController
    {
        public Pet Pet
        {
            get
            {
                var pet = Character as Pet;
                return pet;
            }
        }
    }
}