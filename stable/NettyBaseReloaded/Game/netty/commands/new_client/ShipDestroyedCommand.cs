using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class ShipDestroyedCommand
    {
        public const short ID = 649;

        public static Command write(int destroyedUserId, int explosionTypeId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(destroyedUserId >> 5 | destroyedUserId << 27);
            cmd.Short(-5562);
            cmd.Integer(explosionTypeId << 4 | explosionTypeId >> 28);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}