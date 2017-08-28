namespace NettyBaseReloaded.Game.netty.newcommands
{
	class commandK13 : IServerCommand
	{
		public static int ID = 4017;

        public static short N2K = 2;
        public static short DEFAULT = 0;
        public static short ALLY = 1;

	    public short relation;

		public commandK13(short relation)
		{
		    this.relation = relation;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeShort(relation);
        }
	}
}
