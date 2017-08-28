using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestGiversAvailableCommand
    {
        public const short ID = 29581;

        public static byte[] write(List<QuestGiverModule> questGivers)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(questGivers.Count);
            foreach (var loc in questGivers)
            {
                cmd.AddBytes(loc.write());
            }
            return cmd.ToByteArray();
        }
    }
}
