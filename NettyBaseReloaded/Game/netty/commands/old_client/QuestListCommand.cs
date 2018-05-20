using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestListCommand
    {
        public const short ID = 31889;

        public static Command write(List<QuestSlimInfoModule> list, bool onlyStarter, int maxQuests, int maxEventQuests)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(list.Count);
            foreach (var quest in list)
            {
                cmd.AddBytes(quest.write());
            }
            cmd.Boolean(onlyStarter);
            cmd.Integer(maxQuests);
            cmd.Integer(maxEventQuests);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
