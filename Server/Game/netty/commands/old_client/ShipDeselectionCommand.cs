using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ShipDeselectionCommand
    {
        public const short ID = 6044;

        public static Command write()
        {
            var cmd = new ByteArray(ID);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
