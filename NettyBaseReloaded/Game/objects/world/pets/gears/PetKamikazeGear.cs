using System;
using System.Linq;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.pets.gears
{
    class PetKamikazeGear : PetGear
    {
        public PetKamikazeGear(Pet pet, int level) : base(pet, GearType.KAMIKAZE, level, 1, true)
        {
        }

        public override void Tick()
        {
            if (!LockedIn)
            {
                SearchTarget();
            }
        }

        private bool LockedIn;

        private void SearchTarget()
        {
            var owner = Pet.GetOwner();
            if (owner.Cooldowns.CooldownDictionary.Any(c => c.Value is PetKamikazeCooldown)) return;

            var attackers = owner.Controller.Attack.GetActiveAttackers();
            Character lockedCharacter = null;

            if (owner.Controller.Attack.Attacking)
            {
                lockedCharacter = owner.SelectedCharacter;
            }
            else if (attackers.Count > 0)
            {
                lockedCharacter = attackers.OrderBy(x => x.Position.DistanceTo(owner.Position)).FirstOrDefault();
            }

            if (lockedCharacter != null)
            {
                Pet.Controller.Crash(lockedCharacter, 25000 * Level);
                LockedIn = true;
                //start cooldown
                var cooldown = new PetKamikazeCooldown();
                owner.Cooldowns.Add(cooldown);
                cooldown.Send(owner.GetGameSession());
            }
        }

        public override void SwitchTo(int optParam)
        {
            var owner = Pet.GetOwner();
            if (owner == null)
            {
                Pet.Invalidate();
                return;
            }

            var radius = 250 + (100 * Level-1);
            Pet.Controller.PathFollower.Initiate(owner, radius);
        }

        public override void End()
        {
            LockedIn = false;
        }
    }
}