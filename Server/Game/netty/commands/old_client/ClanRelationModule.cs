using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ClanRelationModule
    {
        public const short ID = 12581;

        public short type = 0;
        public ClanRelationModule(short type)
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
