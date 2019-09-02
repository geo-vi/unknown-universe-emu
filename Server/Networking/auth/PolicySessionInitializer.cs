using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Server.Networking.handlers;

namespace Server.Networking.auth
{
    class PolicySessionInitializer : ChannelInitializer<ISocketChannel>
    {
        protected override void InitChannel(ISocketChannel channel)
        {
            var pipeline = channel.Pipeline;
            pipeline.AddLast(new StringEncoder(), new StringDecoder());
            pipeline.AddLast("PolicyMessageHandler", new PolicyMessageHandler());
        }
    }
}
