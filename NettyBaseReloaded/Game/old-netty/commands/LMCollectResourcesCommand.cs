using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class LMCollectResourcesCommand
    {
        public const short ID = 20626;

        public static byte[] write(LogMessengerPriorityModule priorityMode, List<OreCountModule> contentList)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(priorityMode.write());
            cmd.Integer(contentList.Count);
            foreach (var loc in contentList)
            {
                cmd.AddBytes(loc.write());
            }
            return cmd.ToByteArray();
        }
    }
}
