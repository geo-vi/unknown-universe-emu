using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class HellstormSelectRocketRequest
    {
        public const short ID = 7133;

        public AmmunitionTypeModule rocketType;
        public void readCommand(IByteBuffer bytes)
        {
            var p = new ByteParser(bytes);
            p.readShort();
            rocketType = new AmmunitionTypeModule(p.readShort());
        }
    }
}
