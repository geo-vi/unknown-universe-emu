using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Server.Game;
using Server.Game.netty.commands;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Utils;

namespace Server.Networking
{
    class GameClient
    {
        private IChannelHandlerContext Context;

        public int UserId { get; set; }

        public bool Initialized { get; set; }
        
        public IPEndPoint IpEndPoint => Context.Channel.RemoteAddress as IPEndPoint;

        public GameClient(IChannelHandlerContext context)
        {
            Context = context;
        }

        public async Task Send(byte[] bytes)
        {
            try
            {
                var buffer = PooledByteBufferAllocator.Default.DirectBuffer();
                buffer.WriteBytes(bytes);
                await Context.WriteAndFlushAsync(buffer);
            }
            catch
            {
                Debug.WriteLine("->" + Out.GetCaller());
            }
        }
        
        public async Task Disconnect()
        {
            try
            {
                await Context.CloseAsync();
            }
            catch
            {
                Out.WriteLog("Error disconnecting user from Game", "GAME");
            }
        }

    }
}
