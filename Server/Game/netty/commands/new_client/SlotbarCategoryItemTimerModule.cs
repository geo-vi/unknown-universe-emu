using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class SlotbarCategoryItemTimerModule
    {
        public const short ID = 10021;

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

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(timerState.write());
            cmd.UTF(var14v);
            cmd.Short(-7628);
            cmd.Double(time);
            cmd.Boolean(activatable);
            cmd.Short(19606);
            cmd.Double(maxTime);
            return cmd.Message.ToArray();
        }

        public byte[] write2()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(timerState.write());
            cmd.UTF(var14v);
            cmd.Short(-7628);
            cmd.Double(time);
            cmd.Boolean(activatable);
            cmd.Short(19606);
            cmd.Double(maxTime);
            return cmd.ToByteArray();
        }
    }
}
