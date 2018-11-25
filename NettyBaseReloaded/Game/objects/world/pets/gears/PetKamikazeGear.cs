namespace NettyBaseReloaded.Game.objects.world.pets.gears
{
    class PetKamikazeGear : PetGear
    {
        public PetKamikazeGear(Pet pet, int level) : base(pet, GearType.KAMIKAZE, level, 1, false)
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
        }
    }
}