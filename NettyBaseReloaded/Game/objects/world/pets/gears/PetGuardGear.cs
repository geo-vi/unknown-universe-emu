using System.Linq;

namespace NettyBaseReloaded.Game.objects.world.pets.gears
{
    class PetGuardGear : PetGear
    {
        public PetGuardGear(Pet pet) : base(pet, GearType.GUARD, 1, 1, true)
        {
        }

        private IAttackable Selection => Pet.Selected;
        
        public override void Tick()
        {
            var owner = Pet.GetOwner();
            if (owner == null)
            {
                Pet.Invalidate();
                return;
            }

            var parentAttackers = owner.Controller.Attack.GetActiveAttackers();
            if (parentAttackers.Count == 0 && owner.Controller.Attack.Attacking)
            {
                Pet.Selected = owner.Selected;
            }
            else if (parentAttackers.Count > 0 && !parentAttackers.Contains(Selection))
            {
                Pet.Selected = parentAttackers.First();
            }
    
            Pet.Controller.Attack.Attacking = true;
        }

        public override void SwitchTo(int optParam)
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
            Pet.Controller.Attack
                .Attacking = false;
        }
    }
}