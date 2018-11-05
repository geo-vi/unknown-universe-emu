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
            baseController.Attack.Attacking = true;
        }

        public override void Check()
        {
            CheckAttackables();
            Follow(baseController.Pet.GetOwner());
        }

        public override void End(bool shutdown = false)
        {
            baseController.Attack.Attacking = false;
            baseController.Pet.Selected = null;
            Active = false;
        }

        private void CheckAttackables()
        {
            var owner = baseController.Pet.GetOwner();
            if (owner == null || !baseController.Active) return;

            Character selectedCharacter = null;
            if (owner.Controller.Attack.Attacking)
                selectedCharacter = owner.SelectedCharacter;
            else
            {
                var attackers = owner.Controller.Attack.GetActiveAttackers();
                if (attackers.Count > 0)
                {
                    selectedCharacter = attackers.FirstOrDefault();
                }
            }

            baseController.Pet.Selected = selectedCharacter;
        }
    }
}
