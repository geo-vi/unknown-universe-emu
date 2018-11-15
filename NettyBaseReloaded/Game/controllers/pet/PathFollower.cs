using System;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.pet
{
    class PathFollower : IChecker
    {
        private PetController Controller;

        private bool FollowingActive = false;

        private Character Destination;
        
        private int RadiusOfDestination;
        
        public PathFollower(PetController controller)
        {
            Controller = controller;
        }

        public void Initiate(Character destination, int radius)
        {
            FollowingActive = true;
            Destination = destination;
            RadiusOfDestination = radius;
        }
        
        public void Check()
        {
            if (Destination == null)
            {
                Stop();
                return;
            }
            Follow();
        }

        private void Follow()
        {
            var pet = Controller.Pet;
            var petLastMovement = pet.MovementStartTime;
            var petPos = pet.Position;
            if (petLastMovement.AddSeconds(1) > DateTime.Now && Destination.Position.DistanceTo(petPos) < 500 ||
                !Destination.Moving &&
                Vector.IsPositionInCircle(petPos, MovementController.ActualPosition(Destination), 350)) return;

            MovementController.Move(pet, Vector.GetPosOnCircle(Destination.Position, 350));
        }

        public void Stop()
        {
            FollowingActive = false;
        }
    }
}