using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class LMCollectResourcesCommand
    {
        public const short ID = 20626;
        public static Command write(LogMessengerPriorityModule priorityMode, List<OreCountModule> contentList)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(priorityMode.write());
            cmd.Integer(contentList.Count);
            foreach (var content in contentList)
                cmd.AddBytes(content.write());
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
