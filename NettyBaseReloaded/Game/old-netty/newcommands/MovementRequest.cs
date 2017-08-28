namespace NettyBaseReloaded.Game.netty.newcommands
{
    class MovementRequest : ClientCommand
    {
        public static short ID = 11943;

        public int NewY = 0;
        public int OldY = 0;
        public int NewX = 0;
        public int OldX = 0;

        public MovementRequest(SimpleCommand simpleCommand) : base(ID, simpleCommand) { }

        public override void readCommand()
        {
            this.NewY = readInt();
            this.NewY = (int)(((uint)this.NewY >> 6) | ((uint)this.NewY << 26));
            this.OldY = readInt();
            this.OldY = (int)(((uint)this.OldY << 9) | ((uint)this.OldY >> 23));
            this.NewX = readInt();
            this.NewX = (int)(((uint)this.NewX << 1) | ((uint)this.NewX >> 31));
            this.OldX = readInt();
            this.OldX = (int)(((uint)this.OldX >> 10) | ((uint)this.OldX << 22));
        }
    }
}