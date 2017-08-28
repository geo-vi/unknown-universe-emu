namespace NettyBaseReloaded.Game.netty.newcommands
{
    class LogoutCancelled : ClientCommand
    {
        public static int ID = 10020;

        public LogoutCancelled(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
        }
    }
}