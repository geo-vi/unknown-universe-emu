using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.new_client
{
    class JumpgateCreateCommand
    {
        public const short ID = 30093;

        public static Command write(int gateId, int factionId, int designId, int x, int y, bool isVisible, bool vara3G, List<int> vara3D)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(factionId << 6 | factionId >> 26);
            cmd.Integer(vara3D.Count);
            foreach (var c in vara3D)
            {
                cmd.Integer(c >> 8 | c << 24);
            }
            cmd.Integer(x >> 1 | x << 31);
            cmd.Integer(designId << 1 | designId >> 31);
            cmd.Boolean(isVisible);
            cmd.Boolean(vara3G);
            cmd.Integer(y >> 3 | y << 29);
            cmd.Integer(gateId << 5 | gateId >> 27);
            return new Command(cmd.ToByteArray(), true);
        }

    }
}
