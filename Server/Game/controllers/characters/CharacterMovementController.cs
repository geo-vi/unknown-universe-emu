using Server.Game.managers;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.controllers.characters
{
    class CharacterMovementController : AbstractedSubController
    {
        public virtual void OnMovementStarted()
        {
            Character.SetMovement();
        }
        
        public virtual void OnMovementFinished()
        {
            CharacterStateManager.Instance.RemoveState(Character, CharacterStates.MOVING);
            Character.UnsetMovement();
        }
    }
}