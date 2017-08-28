namespace NettyBaseReloaded.Game.netty.newcommands
{
    class LaserAttackRequest : ClientCommand
    {
        public static int ID = 19306;

        public int selectedId;
        public int x;
        public int y;

        public LaserAttackRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            y = readInt();
            y = (int)(((uint)y << 6) | ((uint)y >> 26));
            selectedId = readInt();
            selectedId = (int)(((uint)selectedId >> 16) | ((uint)selectedId << 16));
            x = readInt();
            x = (int)(((uint)x << 7) | ((uint)x >> 25));
        }
    }
}