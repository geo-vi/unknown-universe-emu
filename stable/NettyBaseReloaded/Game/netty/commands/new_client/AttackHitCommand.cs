using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class AttackHitCommand
    {
        public const short ID = 2540;
        
        public static Command write(AttackTypeModule attackType, int attackerId, int victimId, int victimHitpoints,
            int victimShield, int victimNanohull, int damage, bool skilled)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(damage << 8 | damage >> 24);
            cmd.Integer(victimNanohull << 1 | victimNanohull >> 31);
            cmd.Integer(victimShield >> 6 | victimShield << 26);
            cmd.Integer(victimHitpoints << 7 | victimHitpoints >> 25);
            cmd.AddBytes(attackType.write());
            cmd.Integer(victimId >> 6 | victimId << 26);
            cmd.Integer(attackerId >> 2 | attackerId << 30);
            cmd.Boolean(skilled);
            cmd.Short(-26986);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}