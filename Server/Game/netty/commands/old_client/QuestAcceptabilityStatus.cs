using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class QuestAcceptabilityStatus
    {
        public const short NOT_ACCEPTABLE = 0;

        public const short NOT_STARTED = 1;

        public const short RUNNING = 2;

        public const short COMPLETED = 3;

        public const short ID = 6093;

        public short type;
        public QuestAcceptabilityStatus(short type)
        {
            this.type = type;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(type);
            return cmd.Message.ToArray();
        }
    }
}
