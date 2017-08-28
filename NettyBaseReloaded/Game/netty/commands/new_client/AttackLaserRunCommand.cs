using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class AttackLaserRunCommand
    {
        public const short ID = 4160;

        public static Command write(int attackerId, int targetId, int laserColor, bool isDiminishedBySkillShield, bool skilledLaser)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(targetId << 3 | targetId >> 29);
            cmd.Short(22550);
            cmd.Integer(laserColor >> 8 | laserColor << 24);
            cmd.Boolean(skilledLaser);
            cmd.Integer(attackerId << 7 | attackerId >> 25);
            cmd.Boolean(isDiminishedBySkillShield);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}