namespace NettyBaseReloaded.Game.netty.newcommands.slotbarModules
{
	class TimerState : IServerCommand
	{
		public static int ID = 8825;

        public static short COOLDOWN = 2;
        public static short READY = 0;
        public static short ACTIVE = 1;

	    public short value;

		public TimerState(short value)
		{
		    this.value = value;
		}

	    public override void write()
	    {
            writeShort(ID);
            writeShort(value);
            writeShort(18232);
        }
	}
}
