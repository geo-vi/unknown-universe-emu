namespace NettyBaseReloaded.Game.netty.newcommands
{
    class RocketAttackRequest : ClientCommand
    {
        public static int ID = 22336;

        public int selectedId;
        public int x;
        public int y;

        public RocketAttackRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            readShort();
            selectedId = readInt();
            selectedId = (int)(((uint)selectedId << 4) | ((uint)selectedId >> 28));
            y = readInt();
            y = (int)(((uint)x >> 12) | ((uint)x << 20));
            x = readInt();
            x = (int)(((uint)y >> 16) | ((uint)y << 16));
        }
    }
}