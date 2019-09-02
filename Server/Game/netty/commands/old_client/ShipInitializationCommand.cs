using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class ShipInitializationCommand
    {
        public const short ID = 26642;

        public static Command write(int userId, string userName, int shipType, int speed, int shield, int shieldMax, int hitPoints, int hitMax,
            int cargoSpace, int cargoSpaceMax, int nanoHull, int maxNanoHull, int x, int y, int mapId, int factionId, int clanId, int laserBatteriesMax,
            int rocketsMax, int expansionStage, bool premium, double ep, double honourPoints, int level, double credits, double uridium, float jackpot, int dailyRank,
            string clanTag, int galaxyGatesDone, bool useSystemFont, bool cloaked, List<VisualModifierCommand> modifiers)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.UTF(userName);
            cmd.Integer(shipType);
            cmd.Integer(speed);
            cmd.Integer(shield);
            cmd.Integer(shieldMax);
            cmd.Integer(hitPoints);
            cmd.Integer(hitMax);
            cmd.Integer(cargoSpace);
            cmd.Integer(cargoSpaceMax);
            cmd.Integer(nanoHull);
            cmd.Integer(maxNanoHull);
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Integer(mapId);
            cmd.Integer(factionId);
            cmd.Integer(clanId);
            cmd.Integer(laserBatteriesMax);
            cmd.Integer(rocketsMax);
            cmd.Integer(expansionStage);
            cmd.Boolean(premium);
            cmd.Double(ep);
            cmd.Double(honourPoints);
            cmd.Integer(level);
            cmd.Double(credits);
            cmd.Double(uridium);
            cmd.Float(jackpot);
            cmd.Integer(dailyRank);
            cmd.UTF(clanTag);
            cmd.Integer(galaxyGatesDone);
            cmd.Boolean(useSystemFont);
            cmd.Boolean(cloaked);
            cmd.Integer(modifiers.Count);
            foreach (var modifier in modifiers)
            {
                cmd.AddBytes(modifier.write());
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
