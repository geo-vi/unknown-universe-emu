using System;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Server.Networking.auth;
using Server.Utils;

namespace Server.Networking
{
    abstract class AbstractBootstrapServer
    {
        public int Backlog { get; }
        
        /// <summary>
        /// Port used for server
        /// </summary>
        public int Port { get; }

        private readonly ServerBootstrap _bootstrap;
        private readonly MultithreadEventLoopGroup _bossEventLoopGroup;
        private readonly MultithreadEventLoopGroup _workerEventLoopGroup;
        private IChannel _channel;
        
        /// <summary>
        /// Initializer in every subclass
        /// </summary>
        protected abstract ChannelInitializer<ISocketChannel> Initializer { get; }
        
        protected AbstractBootstrapServer(int port,
            int backlog)
        {
            Port = port;
            Backlog = backlog;

            _bootstrap = new ServerBootstrap();
            _bossEventLoopGroup = new MultithreadEventLoopGroup(1);
            _workerEventLoopGroup = new MultithreadEventLoopGroup();
        }

        private void Initialize()
        {
            if (Initializer == null)
            {
                Out.QuickLog("Abstract server failed to start on Port " + Port + " due to the server's initializer being NULL");
                return;
            }
            
            _bootstrap.Group(_bossEventLoopGroup,
                _workerEventLoopGroup);

            // Set Channel Type to Tcp.
            _bootstrap.Channel<TcpServerSocketChannel>();

            // Set Server Options
            _bootstrap.Option(ChannelOption.SoLinger, 0);
            _bootstrap.Option(ChannelOption.SoBacklog, Backlog);

            _bootstrap.ChildHandler(Initializer);

            _bootstrap.ChildOption(ChannelOption.SoLinger, 0);
            _bootstrap.ChildOption(ChannelOption.SoKeepalive, true);
            _bootstrap.ChildOption(ChannelOption.TcpNodelay, true);
            _bootstrap.ChildOption(ChannelOption.SoReuseaddr, true);
        }


        public async Task StartAsync()
        {
            Initialize();
            _channel = await _bootstrap.BindAsync(Port);
        }

        public async Task StopAsync()
        {
            if (_channel != null) await _channel.CloseAsync();
            if (_bossEventLoopGroup != null && _workerEventLoopGroup != null)
            {
                await _bossEventLoopGroup.ShutdownGracefullyAsync();
                await _workerEventLoopGroup.ShutdownGracefullyAsync();
            }
        }
    }
}