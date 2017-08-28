namespace NettyBaseReloaded.Game.netty.newcommands
{
    class WindowUpdateRequest : ClientCommand
    {
        public static int ID = 13008;

        public string itemId = "";

        public WindowUpdateRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            itemId = readUTF();
        }
    }
}