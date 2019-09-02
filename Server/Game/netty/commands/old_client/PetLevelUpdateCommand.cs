using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetLevelUpdateCommand
    {
        public const short ID = 1057;

        public static Command write(short petLevel, double petExperiencePointsUntilNextLevel, short designId, short expansionStage)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(petLevel);
            cmd.Double(petExperiencePointsUntilNextLevel);
            cmd.Short(designId);
            cmd.Short(expansionStage);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
