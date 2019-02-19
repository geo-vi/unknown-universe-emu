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

        private bool CrashingIn;

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

        public void Initiate(Character destination, bool crashingIn)
        {
            Destination = destination;
            CrashingIn = crashingIn;
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
            if (!FollowingActive) return;

            var pet = Controller.Pet;
            var petLastMovement = pet.MovementStartTime;
            var petPos = pet.Position;
            if (!CrashingIn)
            {
                if (Destination.Moving && pet.InRange(Destination, RadiusOfDestination))
                {
                    MovementController.Move(pet, Vector.GetPosOnCircle(Destination.Destination, RadiusOfDestination));
                    return;
                }
                if (pet.InRange(Destination, RadiusOfDestination) || pet.Destination.DistanceTo(Destination.Position) <= RadiusOfDestination)
                {
                    return;
                }
                MovementController.Move(pet, Vector.GetPosOnCircle(Destination.Position, RadiusOfDestination));
            }
            else
            {
                if (petLastMovement.AddSeconds(1) < DateTime.Now && Destination.InRange(pet))
                {
                    MovementController.Move(pet, Destination.Position);
                }
            }
        }

        public void Stop()
        {
            FollowingActive = false;
            CrashingIn = false;
        }
    }
}