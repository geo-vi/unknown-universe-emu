namespace NettyBaseReloaded.Game.netty.newcommands.killscreenModules
{
	class KillscreenDestroyerModule : IServerCommand
	{
		public static int ID = 12565;

        public static short UNKNOWN = 4;
      
        public static short USER_URL = 0;
      
        public static short RADIATION = 2;
      
        public static short MINE = 3;
      
        public static short USER = 1;
     

	    public short varK3b;

	    public KillscreenDestroyerModule(short varK3B)
	    {
	        varK3b = varK3B;
	    }

	    public override void write()
	    {
	        writeShort(ID);
	        writeShort(varK3b);
	        writeShort(-25426);
	        writeShort(-16151);
	    }
	}
}
