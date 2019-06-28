using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class AttackHitCommand
    {
        public const short ID = 27342;

        public static Command write(AttackTypeModule attackType, int attackerId, int victimId, int victimHitpoints, int victimShield, int victimNanohull,
            int damage, bool skilled)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(attackType.write());
            cmd.Integer(attackerId);
            cmd.Integer(victimId);
            cmd.Integer(victimHitpoints);
            cmd.Integer(victimShield);
            cmd.Integer(victimNanohull);
            cmd.Integer(damage);
            cmd.Boolean(skilled);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}