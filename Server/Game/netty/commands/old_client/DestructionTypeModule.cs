using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class DestructionTypeModule
    {
        public const short PLAYER = 0;

        public const short NPC = 1;

        public const short RADITATION = 2;

        public const short MINE = 3;

        public const short MISC = 4;

        public const short BATTLESTATION = 5;

        public const short ID = 19986;

        public short cause { get; set; }

        public DestructionTypeModule(short cause)
        {
            this.cause = cause;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(cause);
            return cmd.Message.ToArray();
        }
    }
}
