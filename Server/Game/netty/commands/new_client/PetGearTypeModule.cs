using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class PetGearTypeModule
    {
        public const short ID = 27954;

        public static short BEHAVIOR = 0;

        public static short PASSIVE = 1;

        public static short GUARD = 2;

        public static short GEAR = 3;

        public static short AUTO_LOOT = 4;

        public static short AUTO_RESOURCE_COLLECTION = 5;

        public static short ENEMY_LOCATOR = 6;

        public static short RESOURCE_LOCATOR = 7;

        public static short TRADE_POD = 8;

        public static short REPAIR_PET = 9;

        public static short KAMIKAZE = 10;

        public static short COMBO_SHIP_REPAIR = 11;

        public static short COMBO_GUARD = 12;

        public static short ADMIN = 13;

        public short typeValue = 0;

        public PetGearTypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(-24842);
            cmd.Short(this.typeValue);
            return cmd.Message.ToArray();
        }
    }
}
