using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.pets;

namespace NettyBaseReloaded.Game.controllers.pet.gears
{
    class ComboRepairGear : Gear
    {
        public ComboRepairGear(PetController controller) : base(controller, true, 3)
        {
            Type = GearType.COMBO_SHIP_REPAIR;
        }

        private bool Active = false;
        public override void Activate()
        {
            var owner = baseController.Pet.GetOwner();
            if (owner != null)
            {
                if (owner.Cooldowns.Any(x => x is PetComboRepairCooldown)) return;
                Active = true;

                baseController.Heal.HealingId = owner.Id;
                baseController.Heal.Amount = 25000;
                baseController.Heal.HealType = HealType.HEALTH;
                baseController.Heal.Healing = true;
                PulseActivationTime = DateTime.Now;
            }
        }

        public override void Check()
        {
            CheckPulse();
            Follow(baseController.Pet.GetOwner());
        }

        private DateTime PulseActivationTime = new DateTime();
        public void CheckPulse()
        {
            if (PulseActivationTime.AddSeconds(5) > DateTime.Now) return;
            baseController.Heal.Healing = false;
            Active = false;
        }
    }
}
