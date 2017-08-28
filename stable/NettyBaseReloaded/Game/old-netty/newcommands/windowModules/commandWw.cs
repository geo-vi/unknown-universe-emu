namespace NettyBaseReloaded.Game.netty.newcommands.windowModules
{
	class commandWw : IServerCommand
	{
		public static int ID = 3660;

        public static short LO = 7;
      
        public static short n31 = 4;
      
        public static short PLAIN = 0;
      
        public static short x1 = 2;
      
        public static short A36 = 3;
      
        public static short B4t = 1;
      
        public static short LOCALIZED = 5;
      
        public static short Yv = 6;

	    public short varU21;

		public commandWw(short varU21)
		{
		    this.varU21 = varU21;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeShort(varU21);
        }
	}
}
