using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class POIDesignModule
    {
        public const short NONE = 0;

        public const short ASTEROIDS = 1;

        public const short ASTEROIDS_BLUE = 2;

        public const short ASTEROIDS_MIXED_WITH_SCRAP = 3;

        public const short SCRAP = 4;

        public const short NEBULA = 5;

        public const short SIMPLE = 6;

        public const short SECTOR_CONTROL_HOME_ZONE = 7;

        public const short SECTOR_CONTROL_SECTOR_ZONE = 8;

        public const short ID = 7197;

        public short designValue = 0;

        public POIDesignModule(short designValue)
        {
            this.designValue = designValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(designValue);
            cmd.Short(-32102);
            return cmd.Message.ToArray();
        }
    }
}