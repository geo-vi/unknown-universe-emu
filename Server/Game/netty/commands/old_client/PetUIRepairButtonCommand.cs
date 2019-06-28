using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetUIRepairButtonCommand
    {
        public const short ID = 26065;

        public static Command write(bool enabled, int repairCosts)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(enabled);
            cmd.Integer(repairCosts);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
