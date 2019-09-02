using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class AttackMissedCommand
    {
        public const short ID = 25902;

        public static Command write(AttackTypeModule attackType, int targetUserId, int skillColorId)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(attackType.write());
            cmd.Integer(targetUserId);
            cmd.Integer(skillColorId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
