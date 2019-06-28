using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class LogMessengerPriorityModule
    {
        public const short STANDARD = 0;

        public const short HIGH_PRIORITY = 1;

        public const short ID = 61;

        public short priorityModeValue;

        public LogMessengerPriorityModule(short priorityModeValue)
        {
            this.priorityModeValue = priorityModeValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(priorityModeValue);
            return cmd.Message.ToArray();
        }
    }
}
