using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.controllers.pet
{
    abstract class PetSubController : AbstractedSubController
    {
        protected PetSubController(Pet pet) : base(pet)
        {
        }
    }
}