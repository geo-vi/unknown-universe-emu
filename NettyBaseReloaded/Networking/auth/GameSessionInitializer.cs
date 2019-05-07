using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyBaseReloaded.Networking.handlers;

namespace NettyBaseReloaded.Networking.auth
{
    class GameSessionInitializer : ChannelInitializer<ISocketChannel>
    {
        protected override void InitChannel(ISocketChannel channel)
        {
            var pipeline = channel.Pipeline;

            pipeline.AddLast("GameMessageHandler", new GameMessageHandler());
        }
    }
}
