namespace NettyBaseReloaded.Game.netty.newcommands
{
    class KillscreenButtonRequest : ClientCommand
    {
        public static int ID = 24278;

        public short value;

        public KillscreenButtonRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            value = readShort();
            readShort();
        }
    }
}