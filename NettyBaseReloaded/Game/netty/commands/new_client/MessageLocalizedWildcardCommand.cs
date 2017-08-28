using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class MessageLocalizedWildcardCommand
    {
        public const short ID = 19662;

        public string baseKey;
        public commandWw varT1J;
        public List<commandF5> vard3d;

        public MessageLocalizedWildcardCommand(string baseKey, commandWw varT1J, List<commandF5> vard3D)
        {
            this.baseKey = baseKey;
            this.varT1J = varT1J;
            vard3d = vard3D;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(vard3d.Count);
            foreach (var c in vard3d)
            {
                cmd.AddBytes(c.write());
            }
            cmd.Short(-21482);
            cmd.Short(20005);
            cmd.AddBytes(varT1J.write());
            cmd.UTF(baseKey);
            return cmd.Message.ToArray();
        }
    }
}