namespace NettyBaseReloaded.Game.netty.newcommands
{
	class DamageEffectModule : IServerCommand
	{
		public static int ID = 22846;

        public static short KAMIKAZE = 9;
        public static short LASER = 1;
        public static short REPAIR = 10;
        public static short varRl = 14;
        public static short SL = 6;
        public static short ECI = 5;
        public static short ROCKET = 0;
        public static short SINGULARITY = 8;
        public static short PLASMA = 4;
        public static short DECELERATION = 11;
        public static short varyz = 12;
        public static short varFl = 15;
        public static short CID = 7;
        public static short RADIATION = 3;
        public static short var636 = 13;
        public static short MINE = 2;

	    public short value;

		public DamageEffectModule(short value)
		{
		    this.value = value;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeShort(value);
        }
    }
}
