using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class PetHeroActivationCommand
    {
        public const short ID = 31487;

        public static Command write(int ownerId, int petId, short shipType, short expansionStage, string petName, short factionId, int clanId, short petLevel, string clanTag, int x, int y, int petSpeed)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(ownerId);
            cmd.Integer(petId);
            cmd.Short(shipType);
            cmd.Short(expansionStage);
            cmd.UTF(petName);
            cmd.Short(factionId);
            cmd.Integer(clanId);
            cmd.Short(petLevel);
            cmd.UTF(clanTag);
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Integer(petSpeed);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
