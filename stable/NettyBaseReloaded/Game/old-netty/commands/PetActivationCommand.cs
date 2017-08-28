using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class PetActivationCommand
    {
        public const short ID = 8845;

        public static byte[] write(int ownerId, int petId, short petDesignId, short expansionStage, string petName, short petFactionId,
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

            return cmd.ToByteArray();
        }
    }
}
