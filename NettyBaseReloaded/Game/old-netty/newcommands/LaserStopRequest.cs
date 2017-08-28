namespace NettyBaseReloaded.Game.netty.newcommands
{
    class LaserStopRequest : ClientCommand
    {
        public static int ID = 4592;

        public LaserStopRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            readShort();
            readShort();
        }
    }
}