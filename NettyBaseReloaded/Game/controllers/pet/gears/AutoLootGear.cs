using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.controllers.pet.gears
{
    class AutoLootGear : Gear
    {
        internal AutoLootGear(PetController controller) : base(controller, true, 1)
        {
            Type = GearType.AUTO_LOOT;
        }

        public override void Activate()
        {
        }

        public override void Check()
        {
            var pet = baseController.Pet;
            if (pet.Range.Collectables.Count > 0)
            {
                // TODO
                //Console.WriteLine(pet.Range.Collectables.Count + ":PET Boxes count");
                //var collectable = pet.Range.Collectables.FirstOrDefault();
                //collectable.Value.Collect(pet);
                //MovementController.Move(pet, new Vector(collectable.Value.Position.X, collectable.Value.Position.Y - 50));
            }
            else Follow(baseController.Pet.GetOwner());
        }

        public override void End(bool shutdown = false)
        {
        }
    }
}
