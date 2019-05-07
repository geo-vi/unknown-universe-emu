using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class AbilityLaunchRequest
    {
        public const short ID = 26418;

        public int selectedAbilityId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            selectedAbilityId = parser.readInt();
        }
    }
}
