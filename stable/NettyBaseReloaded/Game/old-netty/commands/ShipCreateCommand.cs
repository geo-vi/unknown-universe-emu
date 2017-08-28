using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipCreateCommand
    {
        public const short ID = 24858;
        
        public static byte[] write(int userId, int shipType, int expansionStage, string clanTag,string userName, int x, int y, int factionId, int clanId, int dailyRank,
            bool warnBox, ClanRelationModule clanDiplomacy, int galaxyGatesDone, bool useSystemFont, bool npc, bool cloaked, int motherShipId, int positionIndex, List<VisualModifierCommand> modifiers)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.Integer(shipType);
            cmd.Integer(expansionStage);
            cmd.UTF(clanTag);
            cmd.UTF(userName);
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Integer(factionId);
            cmd.Integer(clanId);
            cmd.Integer(dailyRank);
            cmd.Boolean(warnBox);
            cmd.AddBytes(clanDiplomacy.write());
            cmd.Integer(galaxyGatesDone);
            cmd.Boolean(useSystemFont);
            cmd.Boolean(npc);
            cmd.Boolean(cloaked);
            cmd.Integer(motherShipId);
            cmd.Integer(positionIndex);
            cmd.Integer(modifiers.Count);
            foreach (var modifier in modifiers)
            {
                cmd.AddBytes(modifier.write());
            }
            return cmd.ToByteArray();
        }
    }
}
