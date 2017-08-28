namespace NettyBaseReloaded.Game.netty.newcommands.slotbarModules
{
	class SlotbarCategoryItemTimerModule : IServerCommand
	{
		public static int ID = 10021;

        public TimerState timerState;
        public string var14v = "";
        public double time = 0;
        public bool activatable = false;
        public double maxTime = 0;

		public SlotbarCategoryItemTimerModule(string var14v, TimerState timerState, double time, double maxTime, bool activatable)
		{
		    this.var14v = var14v;
		    this.timerState = timerState;
		    this.time = time;
		    this.maxTime = maxTime;
		    this.activatable = activatable;
		}

	    public override void write()
	    {
            writeShort(ID);
            timerState.write();
            writeBytes(timerState.command.ToArray());
            writeUTF(var14v);
            writeShort(-7628);
            writeDouble(time);
            writeBoolean(activatable);
            writeShort(19606);
            writeDouble(maxTime);
        }
	}
}
