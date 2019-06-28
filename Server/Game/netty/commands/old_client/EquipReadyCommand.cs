using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class EquipReadyCommand
    {
        public const short ID = 31370;

        public static Command write(bool ready)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(ready);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
