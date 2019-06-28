using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.new_client
{
    class ShipInitializationCommand
    {
        public const short ID = 11451;

        public static Command write(int userId, string userName, string shipLootId, int speed, int shield, int shieldMax, int hitPoints, int hitMax, int cargoSpace, int cargoSpaceMax,
            int nanoHull, int maxNanoHull, int x, int y, int mapId, int factionId, int clanId, int expansionStage, bool premium, double ep, double honourPoints, int level,
            double credits, double uridium, float jackpot, int dailyRank, string clanTag, int galaxyGatesDone, bool useSystemFont, bool cloaked, bool var83D, List<VisualModifierCommand> modifiers)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(cargoSpaceMax >> 12 | cargoSpaceMax << 20);
            cmd.Integer(speed >> 11 | speed << 21);
            cmd.UTF(userName);
            cmd.Boolean(var83D);
            cmd.Integer(hitPoints << 1 | hitPoints >> 31);
            cmd.Float(jackpot);
            cmd.Boolean(premium);
            cmd.Integer(clanId >> 11 | clanId << 21);
            cmd.Integer(y << 6 | y >> 26);
            cmd.UTF(shipLootId);
            cmd.Integer(shield >> 9 | shield << 23);
            cmd.Integer(galaxyGatesDone << 5 | galaxyGatesDone >> 27);
            cmd.Short(-27558);
            cmd.UTF(clanTag);
            cmd.Integer(mapId >> 14 | mapId << 18);
            cmd.Integer(shieldMax << 9 | shieldMax >> 23);
            cmd.Integer(level >> 7 | level << 25);
            cmd.Integer(x << 5 | x >> 27);
            cmd.Double(ep);
            cmd.Integer(factionId >> 5 | factionId << 27);
            cmd.Integer(cargoSpace >> 3 | cargoSpace << 29);
            cmd.Double(honourPoints);
            cmd.Double(uridium);
            cmd.Integer(maxNanoHull >> 5 | maxNanoHull << 27);
            cmd.Integer(dailyRank << 13 | dailyRank >> 19);
            cmd.Integer(userId << 11 | userId >> 21);
            cmd.Integer(hitMax << 14 | hitMax >> 18);
            cmd.Integer(modifiers.Count);
            foreach (var modifier in modifiers)
            {
                cmd.AddBytes(modifier.write());
            }
            cmd.Boolean(cloaked);
            cmd.Integer(expansionStage >> 9 | expansionStage << 23);
            cmd.Integer(nanoHull >> 3 | nanoHull << 29);
            cmd.Double(credits);
            cmd.Boolean(useSystemFont);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
