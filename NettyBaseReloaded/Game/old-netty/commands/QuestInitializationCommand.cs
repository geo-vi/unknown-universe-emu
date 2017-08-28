using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class QuestInitializationCommand
    {
        public const short ID = 16927;
        public static byte[] write(QuestDefinitionModule quest)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(quest.write());
            return cmd.ToByteArray();
        }
    }
}
