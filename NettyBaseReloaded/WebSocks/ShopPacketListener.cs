using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace NettyBaseReloaded.WebSocks
{
    class ShopPacketListener
    {
        public static void InitiateListener()
        {
            var wssv = new WebSocketServer("ws://213.32.95.48:666");
            wssv.AddWebSocketService<Shop>("/shoplistener");
            wssv.Start();
        }
    }
}
