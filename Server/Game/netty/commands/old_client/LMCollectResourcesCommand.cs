using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
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
