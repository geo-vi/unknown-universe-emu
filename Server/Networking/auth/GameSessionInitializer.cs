using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;

namespace Server.Networking.auth
{
    class GameSessionInitializer : ChannelInitializer<ISocketChannel>
    {
        protected override void InitChannel(ISocketChannel channel)
        {
            Console.WriteLine("Channel initiated");
        }
    }
}
