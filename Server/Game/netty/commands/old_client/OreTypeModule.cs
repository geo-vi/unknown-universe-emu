using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class OreTypeModule
    {
        public const short ID = 4585;

        public const short PROMETIUM = 0;

        public const short ENDURIUM = 1;

        public const short TERBIUM = 2;

        public const short XENOMIT = 3;

        public const short PROMETID = 4;

        public const short DURANIUM = 5;

        public const short PROMERIUM = 6;

        public const short SEPROM = 7;

        public const short PALLADIUM = 8;

        public short typeValue { get; set; }

        public OreTypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public void read(ByteParser parser)
        {
            parser.readShort();
            typeValue = parser.readShort();
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(typeValue);
            return cmd.Message.ToArray();
        }
    }
}
