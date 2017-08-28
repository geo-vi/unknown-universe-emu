using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class AssetCreateCommand
    {
        public const short ID = 12699;

        public static Command write(AssetTypeModule type, string userName, int factionId, string clanTag, int assetId, int designId, int expansionStage,
        int posX, int posY, int clanId, bool invisible, bool visibleOnWarnRadar, bool detectedByWarnRadar, bool showBubble, ClanRelationModule clanRelation, List<VisualModifierCommand> modifier)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(clanTag);
            cmd.Integer(expansionStage << 1 | expansionStage >> 31);
            cmd.Integer(assetId << 10 | assetId >> 22);
            cmd.AddBytes(clanRelation.write());
            cmd.Boolean(invisible);
            cmd.AddBytes(type.write());
            cmd.Integer(posX << 2 | posX >> 30);
            cmd.Integer(designId << 11 | designId >> 21);
            cmd.Integer(clanId >> 9 | clanId << 23);
            cmd.Integer(posY << 10 | posY >> 22);
            cmd.Boolean(showBubble);
            cmd.Boolean(detectedByWarnRadar);
            cmd.Integer(modifier.Count);
            foreach(var _loc2_ in modifier)
            {
                cmd.AddBytes(_loc2_.write());
            }
            cmd.Integer(factionId << 3 | factionId >> 29);
            cmd.Boolean(visibleOnWarnRadar);
            cmd.UTF(userName);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
