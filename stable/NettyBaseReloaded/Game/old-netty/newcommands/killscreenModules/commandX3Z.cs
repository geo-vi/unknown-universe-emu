namespace NettyBaseReloaded.Game.netty.newcommands.killscreenModules
{
	class commandX3Z : IServerCommand
	{
		public static int ID = 6467;

        public static short CREDITS = 0;
        public static short URIDIUM = 1;
       
        public short type = 0;
        public short amount = 0;

	    public commandX3Z(short type, short amount)
	    {
	        this.type = type;
	        this.amount = amount;
	    }

	    public override void write()
	    {
            writeShort(ID);
            writeShort(this.type);
            writeInt(this.amount >> 11 | this.amount << 21);
            writeShort(7603);
        }
    }
}
