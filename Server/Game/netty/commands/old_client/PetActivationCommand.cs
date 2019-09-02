using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetActivationCommand
    {
        public const short ID = 8845;

        public static Command write(int ownerId, int petId, short petDesignId, short expansionStage, string petName, short petFactionId,
            int petClanID, short petLevel, string clanTag, ClanRelationModule clanRelationship, int x, int y, int petSpeed, bool isInIdleMode,
            bool isVisible)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(ownerId);
            cmd.Integer(petId);
            cmd.Short(petDesignId);
            cmd.Short(expansionStage);
            cmd.UTF(petName);
            cmd.Short(petFactionId);
            cmd.Integer(petClanID);
            cmd.Short(petLevel);
            cmd.UTF(clanTag);
            cmd.AddBytes(clanRelationship.write());
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Integer(petSpeed);
            cmd.Boolean(isInIdleMode);
            cmd.Boolean(isVisible);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}