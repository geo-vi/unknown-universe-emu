using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.controllers.pet.gears
{
    class GuardGear : Gear
    {
        internal GuardGear(PetController controller) : base(controller, true, 1)
        {
            Type = GearType.GUARD;
        }

        public override void Activate()
        {

        }

        public override void Check()
        {
            CheckAttackables();
            Follow(baseController.Pet.GetOwner());
        }

        public override void End()
        {
            baseController.Attack.Attacking = false;
            baseController.Pet.Selected = null;
        }

        private void CheckAttackables()
        {
            var owner = baseController.Pet.GetOwner();
            if (owner != null)
            {
                if (baseController.Pet.Selected != null && baseController.Attack.Attacking) return;
                var ownerAttackers = owner.Controller.Attack.GetActiveAttackers();
                if (ownerAttackers.Count > 0)
                {
                    var selected = ownerAttackers.FirstOrDefault();
                    if (selected != null)
                        baseController.Pet.Selected = selected;
                }
            }
        }
    }
}
