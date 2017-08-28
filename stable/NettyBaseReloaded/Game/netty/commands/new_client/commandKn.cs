using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class commandKn
    {
        public const short ID = 3444;

        public static short GENERIC_FEATURE_BAR = 2;

        public static short NOT_ASSIGNED = 0;

        public static short GAME_FEATURE_BAR = 1;

        public List<WindowButtonModule> varh1j;

        public string varA39 = "";

        public short varJ2d = 0;

        public string varh2J = "";

        public commandKn(short varJ2d, List<WindowButtonModule> varh1j, string varh2J, string varA39)
        {
            this.varJ2d = varJ2d;
            this.varh1j = varh1j;
            this.varh2J = varh2J;
            this.varA39 = varA39;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(varh1j.Count);
            foreach (var c in varh1j)
            {
                cmd.AddBytes(c.write());
            }
            cmd.UTF(varA39);
            cmd.Short(varJ2d);
            cmd.UTF(varh2J);
            cmd.Short(-15817);
            cmd.Short(13250);
            return cmd.Message.ToArray();
        }

    }
}
