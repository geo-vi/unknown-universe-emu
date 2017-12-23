using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class DisplaySettingsRequest
    {
        public const short ID = 15703;

        //TODO

        public void readCommand(byte[] bytes)
        {
            var cmd = new ByteParser(bytes);
        }
    }
}
