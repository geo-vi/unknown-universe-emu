using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class POITypeModule
    {
        public const short GENERIC = 0;

        public const short FACTORIZED = 1;

        public const short TRIGGER = 2;

        public const short DAMAGE = 3;

        public const short HEALING = 4;

        public const short NO_ACCESS = 5;

        public const short FACTION_NO_ACCESS = 6;

        public const short ENTER_LEAVE = 7;

        public const short RADIATION = 8;

        public const short CAGE = 9;

        public const short MINE_FIELD = 10;

        public const short BUFF_ZONE = 11;

        public const short SECTOR_CONTROL_HOME_ZONE = 12;

        public const short SECTOR_CONTROL_SECTOR_ZONE = 13;

        public const short ID = 10771;

        public short typeValue = 0;

        public POITypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(typeValue);
            return cmd.Message.ToArray();
        }
    }
}