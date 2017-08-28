namespace NettyBaseReloaded.Game.netty.newcommands
{
    class LegacyModuleRequest : ClientCommand
    {
        public static int ID = 32601;

        public string message;

        public LegacyModuleRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            this.message = readUTF();
        }
    }
}