using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class DestructionTypeModule
    {
        public const short ID = 12565;

        public const short USER_URL = 0;

        public const short USER = 1;

        public const short RADIATION = 2;

        public const short MINE = 3;

        public const short UNKNOWN = 4;

        public short cause;

        public DestructionTypeModule(short cause)
        {
            this.cause = cause;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(cause);
            cmd.Short(-25426);
            cmd.Short(-16151);
            return cmd.Message.ToArray();
        }

    }
}