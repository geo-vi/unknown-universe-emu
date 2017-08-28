namespace NettyBaseReloaded.Game.netty.newcommands.slotbarModules
{
	class CooldownTypeModule : IServerCommand
	{
		public static int ID = 3004;

        public static short ENERGY_CHAIN_IMPULSE = 14;
        public static short varY8 = 24;
        public static short BATTLE_REPAIR_BOT = 18;
        public static short vara2E = 12;
        public static short SPEED_BUFF = 35;
        public static short varY1t = 26;
        public static short MINE = 1;
        public static short varf2D = 30;
        public static short vari4Y = 6;
        public static short varT1L = 36;
        public static short SHIELD_BACKUP = 16;
        public static short varf3Q = 25;
        public static short varo1O = 31;
        public static short varY1y = 5;
        public static short varB4u = 40;
        public static short varU3s = 9;
        public static short varF2B = 37;
        public static short vara4h = 43;
        public static short varK46 = 29;
        public static short varM4H = 33;
        public static short ENERGY_LEECH_ARRAY = 13;
        public static short var338 = 39;
        public static short SPEED_LEECH = 17;
        public static short vara1L = 41;
        public static short varo3g = 19;
        public static short vare3n = 32;
        public static short SINGULARITY = 34;
        public static short vara3E = 28;
        public static short varJF = 27;
        public static short PLASMA = 7;
        public static short varJY = 8;
        public static short varNx = 2;
        public static short varA2P = 45;
        public static short var544 = 42;
        public static short NONE = 0;
        public static short varYt = 38;
        public static short ROCKET_PROBABILITY_MAXIMIZER = 15;
        public static short varX30 = 22;
        public static short varNM = 10;
        public static short varx37 = 3;
        public static short vare1K = 20;
        public static short ROCKET = 4;
        public static short ROCKET_LAUNCHER = 11;
        public static short vare7 = 44;
        public static short varK2H = 23;
        public static short varFn = 21;

	    public short value;

		public CooldownTypeModule(short value)
		{
		    this.value = value;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeShort(value);
            writeShort(-6047);
            writeShort(9032);
        }
	}
}
