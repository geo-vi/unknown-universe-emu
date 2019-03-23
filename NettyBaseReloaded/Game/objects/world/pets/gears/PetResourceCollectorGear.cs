using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Game.objects.world.pets.gears
{
    class PetResourceCollectorGear : PetGear
    {
        public PetResourceCollectorGear(Pet pet, int level) : base(pet, GearType.AUTO_RESOURCE_COLLECTION, level, 1, true)
        {
        }

        public override void Tick()
        {
            if (CollectionStarted.AddSeconds(2) > DateTime.Now)
            {
                return;
            }

            if (LockedOre?.Position == null)
            {
                SearchOres();
            }
            else
            {
                TravelToOre();
            }
        }

        private Ore LockedOre;

        private void SearchOres()
        {
            var ore = Pet.Range.Resources.Values.Where(x => x.Position != null && !Pet.GetOwner().Information.Cargo.Full).OrderBy(x => x.Position.DistanceTo(Pet.Position))
                .FirstOrDefault();

            var owner = Pet.GetOwner();

            var autoLooterRange = 2000;
            switch (Level)
            {
                case 2:
                    autoLooterRange = 2500;
                    break;
                case 3:
                    autoLooterRange = 3000;
                    break;
            }

            if (ore != null && owner.Position.DistanceTo(ore.Position) < autoLooterRange)
            {
                LockedOre = ore;
                Pet.Controller.PathFollower.Stop();
                MovementController.Move(Pet, new Vector(LockedOre.Position.X, LockedOre.Position.Y - 100));
            }
            else
            {
                Fly();
            }
        }

        private DateTime CollectionStarted = DateTime.Now;

        private void TravelToOre()
        {
            if (Pet.Position.DistanceTo(LockedOre.Position) <= 150)
            {
                CollectionStarted = DateTime.Now;
                LockedOre.Collect(Pet);
                LockedOre = null;
            }
            else if (LockedOre.Position.DistanceTo(Pet.Destination) > 150)
            {
                LockedOre = null;
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
