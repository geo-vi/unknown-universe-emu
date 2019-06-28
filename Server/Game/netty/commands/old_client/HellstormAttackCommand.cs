using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class HellstormAttackCommand
    {
        public const short ID = 24152;

        public static Command write(int attackerId, int targetId, bool hit, int currentLoad, AmmunitionTypeModule rocketType)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(attackerId);
            cmd.Integer(targetId);
            cmd.Boolean(hit);
            cmd.Integer(currentLoad);
            cmd.AddBytes(rocketType.write());
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
