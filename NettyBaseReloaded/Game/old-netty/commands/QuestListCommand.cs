using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestListCommand
    {
        public const short ID = 31889;

        public static byte[] write(List<QuestSlimInfoModule> list, bool onlyStarter, int maxQuests, int maxEventQuests)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(list.Count);
            foreach (var loc0 in list)
            {
                cmd.AddBytes(loc0.write());
            }
            cmd.Boolean(onlyStarter);
            cmd.Integer(maxQuests);
            cmd.Integer(maxEventQuests);
            return cmd.ToByteArray();
        }
    }
}
