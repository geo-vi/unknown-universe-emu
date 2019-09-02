using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ShipWarpCompletedCommand
    {
        public const short ID = 5630;

        public static Command write()
        {
            var cmd = new ByteArray(ID);
            return new Command(cmd.ToByteArray(), false);
        }

    }
}
