using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Game.objects.world.pets.gears
{
    class PetCollectorGear : PetGear
    {
        public PetCollectorGear(Pet pet, int level) : base(pet, GearType.AUTO_LOOT, level, 1, true)
        {
        }

        public override void Tick()
        {
            if (CollectionStarted.AddSeconds(2) > DateTime.Now)
            {
                return;
            }

            if (LockedCollectable?.Position == null)
            {
                SearchCollectable();
            }
            else
            {
                TravelToCollectable();
            }
        }

        private Collectable LockedCollectable;

        private void SearchCollectable()
        {
            var collectable = Pet.Range.Collectables.Values.Where(x => x.Position != null && x.PetCanCollect(Pet.GetOwner()) && !x.Disposed).OrderBy(x => x.Position.DistanceTo(Pet.Position))
                .FirstOrDefault();

            var owner = Pet.GetOwner();

            var autoLooterRange = 1500;
            switch (Level)
            {
                case 2:
                    autoLooterRange = 1700;
                    break;
                case 3:
                    autoLooterRange = 3000;
                    break;
            }

            if (collectable != null && owner.Position.DistanceTo(collectable.Position) < autoLooterRange)
            {
                LockedCollectable = collectable;
                Pet.Controller.PathFollower.Stop();
                MovementController.Move(Pet, new Vector(LockedCollectable.Position.X, LockedCollectable.Position.Y - 100));
            }
            else
            {
                Fly();
            }
        }

        private DateTime CollectionStarted = DateTime.Now;

        private void TravelToCollectable()
        {
            if (LockedCollectable.PetCanCollect(Pet.GetOwner()) && !LockedCollectable.Disposed && LockedCollectable.Position != null && Pet.Position.DistanceTo(LockedCollectable.Position) <= 150)
            {
                CollectionStarted = DateTime.Now;
                LockedCollectable.Collect(Pet);
                LockedCollectable = null;
            }
            else
            {
                LockedCollectable = null;
            }
        }

        public override void SwitchTo(int optParam)
        {
            Fly();
        }

        private void Fly()
        {
            var owner = Pet.GetOwner();
            if (owner == null)
            {
                Pet.Invalidate();
                return;
            }
            Pet.Controller.PathFollower.Initiate(owner, 350);
        }

        public override void End()
        {
        }
    }
}
