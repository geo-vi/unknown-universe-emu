using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Server.Networking.auth;

namespace Server.Networking.servers
{
    class GameServer : AbstractBootstrapServer
    {
        protected override ChannelInitializer<ISocketChannel> Initializer => new GameSessionInitializer();

        public GameServer() : base(Configurations.ServerConfiguration.GAMESERVER_PORT, 10)
        {
        }
    }
}
