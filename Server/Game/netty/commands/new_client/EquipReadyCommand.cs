using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class EquipReadyCommand
    {
        public const short ID = 26559;

        public static Command write(bool ready)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(ready);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
