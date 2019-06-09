using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Networking.handlers
{
    class GameMessageHandler : ChannelHandlerAdapter
    {
        public GameClient Client;

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer == null) return;
            Packet.Handler.LookUp(buffer, Client);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine($"Exception: {exception}");
            context.CloseAsync();
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Client = new GameClient(context);
            base.ChannelActive(context);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            if (Client != null && Client.UserId != 0)
            {
                World.StorageManager.GetGameSession(Client.UserId)?.Kick();
            }
        }
    }
}
