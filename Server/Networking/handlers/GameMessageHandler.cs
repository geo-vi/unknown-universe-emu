using System;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Server.Game;
using Server.Game.netty;
using Server.Main.objects;
using Server.Networking.clients;
using Server.Utils;

namespace Server.Networking.handlers
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

        /// <summary>
        /// Exception caught, closing the connection!
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Out.WriteLog($"Socket exception caught: {exception}, closing..", LogKeys.ERROR_LOG);
            context.CloseAsync();
        }

        /// <summary>
        /// Client is connected, creating a new GameClient instance
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Client = new GameClient(context);
            base.ChannelActive(context);
        }

        /// <summary>
        /// Channel goes inactive, calling client method to announce
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Client?.OnClientConnectionClosed();
        }
    }
}
