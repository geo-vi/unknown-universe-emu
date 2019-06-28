using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetExperiencePointsUpdateCommand
    {
        public const short ID = 2318;

        public static Command write(double currentExperiencePoints, double maxExperiencePoints)
        {
            var cmd = new ByteArray(ID);
            cmd.Double(currentExperiencePoints);
            cmd.Double(maxExperiencePoints);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}