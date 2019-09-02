using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using DotNetty.Codecs;
using Server.Networking.handlers;

namespace Server.Networking.auth
{
    class GameSessionInitializer : ChannelInitializer<ISocketChannel>
    {
        protected override void InitChannel(ISocketChannel channel)
        {
            var pipeline = channel.Pipeline;
            pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
            pipeline.AddLast("GameMessageHandler", new GameMessageHandler());
        }
    }
}
