using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.controllers.pet.gears
{
    class Guard : Gear
    {
        internal Guard(PetController controller) : base(controller, true, 1)
        {
            Type = GearType.GUARD;
        }

        public override void Activate()
        {
            var ownerSelection = baseController.Pet.GetOwner().Selected as Npc;
            if (ownerSelection != null)
            {
                baseController.Pet.Selected = ownerSelection;
                baseController.Attack.Attacking = true;
                baseController.Attack.LaserAttack();
            }
        }

        public override void Check()
        {
            Follow(baseController.Pet.GetOwner());
        }
    }
}
