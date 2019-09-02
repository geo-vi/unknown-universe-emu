using Server.Game.objects.entities;

namespace Server.Game.controllers.implementable
{
    abstract class ImplementableController
    {
        protected Character Character;
        
        protected ImplementableController(Character character)
        {
            Character = character;
        }

        public abstract void Tick();
    }
}