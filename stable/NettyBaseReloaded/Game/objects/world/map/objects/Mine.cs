namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    abstract class Mine : Object
    {
        protected Mine(int id, Vector pos) : base(id, pos)
        {

        }

        public abstract override void execute(Character character);
    }
}