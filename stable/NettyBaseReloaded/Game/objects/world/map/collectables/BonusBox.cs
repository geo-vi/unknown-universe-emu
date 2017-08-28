namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class BonusBox : Collectable
    {
        public BonusBox(string hash, Vector pos) : base(hash, Types.BONUS_BOX, pos)
        {

        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public override void Reward()
        {
            throw new System.NotImplementedException();
        }
    }
}