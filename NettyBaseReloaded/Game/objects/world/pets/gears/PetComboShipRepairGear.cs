using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.pets.gears
{
    class PetComboShipRepairGear : PetGear
    {
        public PetComboShipRepairGear(Pet pet, int level) : base(pet, GearType.COMBO_SHIP_REPAIR, level, 1, false)
        {
        }

        public override void Tick()
        {
        }

        public override void SwitchTo(int optParam)
        {
            var owner = Pet.GetOwner();
            if (owner != null)
            {
                var ownerSession = owner.GetGameSession();
                if (ownerSession != null && ownerSession.Active)
                {
                    Packet.Builder.LegacyModule(ownerSession, "0|A|STD|Still work in progress, switching back to Passive gear");
                    Pet.Controller.SwitchGear(GearType.PASSIVE,0);
                }
            }
        }

        public override void End()
        {
        }
    }
}