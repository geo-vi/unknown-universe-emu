using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class AttackTypeModule
    {
        public const short ID = 22846;

        public const short ROCKET = 0;

        public const short LASER = 1;

        public const short MINE = 2;

        public const short RADIATION = 3;

        public const short PLASMA = 4;

        public const short ECI = 5;

        public const short SL = 6;

        public const short CID = 7;

        public const short SINGULARITY = 8;

        public const short KAMIKAZE = 9;

        public const short REPAIR = 10;

        public const short DECELERATION = 11;

        public const short SHIELD_ABSORBER_ROCKET_CREDITS = 12;

        public const short SHIELD_ABSORBER_ROCKET_URIDIUM = 13;

        public const short STICKY_BOMB = 14;

        public const short varFl = 15;

        public short type;

        public AttackTypeModule(short type)
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