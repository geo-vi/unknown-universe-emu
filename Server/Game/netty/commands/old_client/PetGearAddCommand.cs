using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetGearAddCommand
    {
        public const short ID = 29895;

        public static Command write(PetGearTypeModule gearType, int level, int amount, bool enabled)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(gearType.write());
            cmd.Integer(level);
            cmd.Integer(amount);
            cmd.Boolean(enabled);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
