using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class PetStatusCommand
    {
        public const short ID = 17590;

        public static Command write(int petId, int petLevel, double petExperiencePoints, double petExperiencePointsUntilNextLevel, int petHitPoints,
            int petHitPointsMax, int petShieldEnergyNow, int petShieldEnergyMax, int petCurrentFuel, int petMaxFuel, int petSpeed, string petName)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petShieldEnergyMax << 7 | petShieldEnergyMax >> 25);
            cmd.Integer(petId >> 6 | petId << 26);
            cmd.Double(petExperiencePointsUntilNextLevel);
            cmd.Integer(petHitPoints << 8 | petHitPoints >> 24);
            cmd.UTF(petName);
            cmd.Integer(petSpeed << 7 | petSpeed >> 25);
            cmd.Integer(petMaxFuel >> 6 | petMaxFuel << 26);
            cmd.Double(petExperiencePoints);
            cmd.Integer(petShieldEnergyNow >> 10 | petShieldEnergyNow << 22);
            cmd.Integer(petCurrentFuel << 12 | petCurrentFuel >> 20);
            cmd.Integer(petLevel << 14 | petLevel >> 18);
            cmd.Integer(petHitPointsMax << 8 | petHitPointsMax >> 24);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}