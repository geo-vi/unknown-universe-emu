using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Server.Main;
using Server.Networking.auth;

namespace Server.Networking.servers
{
    class PolicyServer : AbstractBootstrapServer
    {
        protected override ChannelInitializer<ISocketChannel> Initializer => new PolicySessionInitializer();

        public PolicyServer() : base(Configurations.ServerConfiguration.POLICY_PORT, 10)
        {
        }
    }
}
