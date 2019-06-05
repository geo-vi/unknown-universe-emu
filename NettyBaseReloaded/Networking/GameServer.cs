using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Networking.auth;

namespace NettyBaseReloaded.Networking
{
    class GameServer
    {
        public int Backlog { get; }
        public int Port { get; }

        private readonly ServerBootstrap _bootstrap;
        private readonly MultithreadEventLoopGroup _bossEventLoopGroup;
        private readonly MultithreadEventLoopGroup _workerEventLoopGroup;
        private IChannel _channel;

        public GameServer(int port,
            int backlog)
        {
            Port = port;
            Backlog = backlog;

            _bootstrap = new ServerBootstrap();
            _bossEventLoopGroup = new MultithreadEventLoopGroup(1);
            _workerEventLoopGroup = new MultithreadEventLoopGroup();
        }

        protected void Initialize()
        {
            _bootstrap.Group(_bossEventLoopGroup,
                _workerEventLoopGroup);

            // Set Channel Type to Tcp.
            _bootstrap.Channel<TcpServerSocketChannel>();

            // Set Server Options
            _bootstrap.Option(ChannelOption.SoLinger, 0);
            _bootstrap.Option(ChannelOption.SoBacklog, Backlog);

            _bootstrap.ChildHandler(new GameSessionInitializer());

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

        private void Init()
        {
            LoadAssets();
            //SpawnAll();

        }

        private void LoadAssets()
        {
            Console.WriteLine("##Starting Asset Loading##");
            //HERE LOAD ALL YOUR DATABASE STUFF TO LISTS IN SESSIONMANAGER
            //ONCE LOADED STUFF; SPAWN ALL STUFF; START THEYRE ROUTES ETC

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
