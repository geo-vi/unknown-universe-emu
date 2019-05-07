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
    class WebSocketReceiver : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var packetData = e.Data;
            //Console.WriteLine(packetData + "[ws]");
            Handler.Handle(this, packetData);
        }

        public new void Send(string msg)
        {
            base.Send(msg);
        }
    }
}
