using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ShipSelectionCommand
    {
        public const short ID = 19586;

        public static Command write(int userId, int shipType, int shield, int shieldMax, int hitpoints,
            int hitpointsMax, int nanoHull, int maxNanoHull,
            bool shieldSkill)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.Integer(shipType);
            cmd.Integer(shield);
            cmd.Integer(shieldMax);
            cmd.Integer(hitpoints);
            cmd.Integer(hitpointsMax);
            cmd.Integer(nanoHull);
            cmd.Integer(maxNanoHull);
            cmd.Boolean(shieldSkill);
            return new Command(cmd.ToByteArray(), false);
        }

    }
}
