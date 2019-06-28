using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class ShipSelectionCommand
    {
        public const short ID = 10954;

        public static Command write(int userId, int shipType, int shield, int shieldMax, int hitpoints,
            int hitpointsMax, int nanoHull, int nanoHullMax, bool shieldSkill)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(nanoHullMax >> 13 | nanoHullMax << 19);
            cmd.Integer(userId >> 11 | userId << 21);
            cmd.Integer(shipType << 7 | shipType >> 25);
            cmd.Integer(nanoHull << 3 | nanoHull >> 29);
            cmd.Integer(hitpointsMax >> 12 | hitpointsMax << 20);
            cmd.Integer(hitpoints >> 16 | hitpoints << 16);
            cmd.Integer(shield << 7 | shield >> 25);
            cmd.Short(-12272);
            cmd.Integer(shieldMax >> 12 | shieldMax << 20);
            cmd.Boolean(shieldSkill);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
