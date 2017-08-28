namespace NettyBaseReloaded.Game.netty.newcommands
{
    class ItemSelectionRequest : ClientCommand
    {
        public static int ID = 2287;

        public static short var92j = 0;
        public static short varR4M = 1;
        public static short SELECT = 0;
        public static short ACTIVATE = 1;

        public short y2g = 0;
        public short var54i = 0;
        public string itemId = "";

        public ItemSelectionRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            readShort();
            this.y2g = readShort();
            this.var54i = readShort();
            this.itemId = readUTF();
        }
    }
}