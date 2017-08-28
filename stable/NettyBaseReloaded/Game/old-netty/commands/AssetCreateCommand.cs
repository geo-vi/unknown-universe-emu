using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AssetCreateCommand
    {
        public const short ID = 19926;

        public static byte[] write(AssetTypeModule type, string userName, int factionId, string clanTag, int assetId, int designId,
            int expansionStage, int posX, int posY, int clanId, bool invisible, bool visibleOnWarnRadar, bool detectedByWarnRadar,
            ClanRelationModule clanRelation, List<VisualModifierCommand> modifier)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(type.write());
            cmd.UTF(userName);
            cmd.Integer(factionId);
            cmd.UTF(clanTag);
            cmd.Integer(assetId);
            cmd.Integer(designId);
            cmd.Integer(expansionStage);
            cmd.Integer(posX);
            cmd.Integer(posY);
            cmd.Integer(clanId);
            cmd.Boolean(invisible);
            cmd.Boolean(visibleOnWarnRadar);
            cmd.Boolean(detectedByWarnRadar);
            cmd.AddBytes(clanRelation.write());
            cmd.Integer(modifier.Count);
            foreach (var _loc2_ in modifier)
            {
                cmd.AddBytes(_loc2_.write());
            }
            return cmd.ToByteArray();
        }
    }
}
