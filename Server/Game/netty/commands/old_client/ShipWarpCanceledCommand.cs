using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ShipWarpCanceledCommand
    {
        public const short ID = 27080;

        public static Command write()
        {
            var cmd = new ByteArray(ID);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
