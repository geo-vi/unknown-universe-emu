using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class KillScreenRepairRequest
    {
        public const short ID = 3303;

        public KillScreenOptionTypeModule selection;

        public void readCommand(byte[] bytes)
        {
            var p = new ByteParser(bytes);
            p.readShort();
            selection = new KillScreenOptionTypeModule(p.readShort());
        }
    }
}
