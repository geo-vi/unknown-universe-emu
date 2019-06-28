using Server.Game.objects.entities;

namespace Server.Game.controllers.characters
{
    abstract class AbstractedSubController
    {
        protected Character Character;
        
        protected AbstractedSubController(Character character)
        {
            Character = character;
        }
    }
}