using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class ClanRelationModule
    {
        public const short ID = 2755;

        public const short NONE = 0;

        public const short ALLIED = 1;

        public const short NON_AGGRESSION_PACT = 2;

        public const short AT_WAR = 3;

        public short type;

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
