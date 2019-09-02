using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.new_client
{
    class ShipCreateCommand
    {
        public const short ID = 25118;

        public static Command write(int userId, string shipType, int expansionStage, string clanTag, string userName, int x, int y,
            int factionId, int clanId, int dailyRank, bool warnBox, ClanRelationModule varT1o, int galaxyGatesDone, bool useSystemFont,
            bool npc, bool cloaked, int motherShipId, int positionIndex, List<VisualModifierCommand> modifier, commandK13 varP1K)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(x >> 2 | x << 30);
            cmd.Integer(motherShipId >> 9 | motherShipId << 23);
            cmd.UTF(userName);
            cmd.Integer(userId << 6 | userId >> 26);
            cmd.AddBytes(varP1K.write());
            cmd.UTF(clanTag);
            cmd.Integer(clanId << 11 | clanId >> 21);
            cmd.Boolean(warnBox);
            cmd.Integer(expansionStage >> 12 | expansionStage << 20);
            cmd.Boolean(npc);
            cmd.Boolean(useSystemFont);
            cmd.Integer(galaxyGatesDone << 7 | galaxyGatesDone >> 25);
            cmd.Integer(positionIndex >> 2 | positionIndex << 30);
            cmd.Boolean(cloaked);
            cmd.AddBytes(varT1o.write());
            cmd.Integer(factionId << 13 | factionId >> 19);
            cmd.Integer(modifier.Count);
            foreach (var c in modifier)
            {
                cmd.AddBytes(c.write());
            }
            cmd.Integer(dailyRank >> 3 | dailyRank << 29);
            cmd.Integer(y >> 15 | y << 17);
            cmd.UTF(shipType);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
