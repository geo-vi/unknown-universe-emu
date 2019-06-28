using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class HitpointInfoCommand
    {
        public const short ID = 30056;

        public static Command write(int hitpoints, int hitpointsMax, int nanoHull, int nanoHullMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(hitpoints);
            cmd.Integer(hitpointsMax);
            cmd.Integer(nanoHull);
            cmd.Integer(nanoHullMax);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}