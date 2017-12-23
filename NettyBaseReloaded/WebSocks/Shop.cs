using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.WebSocks.packets;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace NettyBaseReloaded.WebSocks
{
    class Shop : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var packetData = e.Data;
            Handler.Handle(packetData);
        }
    }
}
