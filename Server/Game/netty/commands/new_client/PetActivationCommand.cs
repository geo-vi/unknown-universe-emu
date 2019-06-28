using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class PetActivationCommand
    {
        public const short ID = 23112;

        public static Command write(int ownerId, int petId, short petDesignId, short expansionStage, string petName,
            short petFactionId, int petClanID, short petLevel, string clanTag, ClanRelationModule clanRelationship,
            int x, int y, int petSpeed, bool isInIdleMode, bool isVisible, commandK13 P1K)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(y >> 12 | y << 20);
            cmd.Short(expansionStage);
            cmd.Integer(x << 1 | x >> 31);
            cmd.Boolean(isVisible);
            cmd.Integer(ownerId << 7 | ownerId >> 25);
            cmd.Integer(petClanID >> 9 | petClanID << 23);
            cmd.Integer(petId << 5 | petId >> 27);
            cmd.Boolean(isInIdleMode);
            cmd.Integer(petSpeed >> 4 | petSpeed << 28);
            cmd.UTF(clanTag);
            cmd.Short(petLevel);
            cmd.Short(petDesignId);
            cmd.Short(petFactionId);
            cmd.UTF(petName);
            cmd.AddBytes(P1K.write());
            cmd.AddBytes(clanRelationship.write());
            return new Command(cmd.ToByteArray(), true);
        }
    }
}