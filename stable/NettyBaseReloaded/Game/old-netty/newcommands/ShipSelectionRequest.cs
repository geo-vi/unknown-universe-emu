namespace NettyBaseReloaded.Game.netty.newcommands
{
    class ShipSelectionRequest : ClientCommand
    {
        public static int ID = 12642;

        public int selectedId = 0;
        public int selectedX = 0;
        public int selectedY = 0;
        public int x = 0;
        public int y = 0;

        public ShipSelectionRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            this.selectedId = readInt();
            this.selectedId = (int)(((uint)this.selectedId << 2) | ((uint)this.selectedId >> 30));
            this.selectedY = readInt();
            this.selectedY = (int)(((uint)this.selectedY >> 15) | ((uint)this.selectedY << 17));
            this.y = readInt();
            this.y = (int)(((uint)this.y << 2) | ((uint)this.y >> 30));
            this.selectedX = readInt();
            this.selectedX = (int)(((uint)this.selectedX >> 5) | ((uint)this.selectedX << 27));
            this.x = readInt();
            this.x = (int)(((uint)this.x >> 2) | ((uint)this.x << 30));
        }
    }
}