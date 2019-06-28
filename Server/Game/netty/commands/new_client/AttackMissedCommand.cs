using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class AttackMissedCommand
    {
        public const short ID = 31887;

        public static Command write(AttackTypeModule attackType, int targetUserId, int skillColorId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(targetUserId << 13 | targetUserId >> 19);
            cmd.AddBytes(attackType.write());
            cmd.Short(-12946);
            cmd.Integer(skillColorId << 3 | skillColorId >> 29);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
