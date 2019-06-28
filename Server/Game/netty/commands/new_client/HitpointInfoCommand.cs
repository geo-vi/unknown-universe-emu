using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class HitpointInfoCommand
    {
        public const short ID = 19181;

        public static Command write(int hitpoints, int hitpointsMax, int nanoHull, int nanoHullMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(hitpoints >> 5 | hitpoints << 27);
            cmd.Integer(hitpointsMax >> 8 | hitpointsMax << 24);
            cmd.Integer(nanoHull >> 10 | nanoHull << 22);
            cmd.Integer(nanoHullMax << 8 | nanoHullMax >> 24);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}