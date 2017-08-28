namespace NettyBaseReloaded.Game.netty.newcommands
{
	class ClanRelationModule : IServerCommand
	{
		public static int ID = 2755;

        public static short WAR = 3;
        public static short PNA = 2;
        public static short ALLIANCE = 1;
        public static short NONE = 0;

	    public short type;

		public ClanRelationModule(short type)
		{
		    this.type = type;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeShort(type);
        }
	}
}
