using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ShipDestroyedCommand
    {
        public const short ID = 11189;

        public static Command write(int destroyedUserId, int explosionTypeId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(destroyedUserId);
            cmd.Integer(explosionTypeId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}