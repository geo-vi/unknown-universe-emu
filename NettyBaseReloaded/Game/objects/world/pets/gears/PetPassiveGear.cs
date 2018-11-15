namespace NettyBaseReloaded.Game.objects.world.pets.gears
{
    class PetPassiveGear : PetGear
    {
        public PetPassiveGear(Pet pet) : base(pet, GearType.PASSIVE, 1, 1, true)
        {
        }

        public override void Tick()
        {
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
            Pet.Controller.PathFollower.Stop();
        }
    }
}