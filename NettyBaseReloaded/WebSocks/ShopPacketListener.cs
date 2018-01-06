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
            var wssv = new WebSocketServer(666);
            //test1 : var wssv = new WebSocketServer(666);
            wssv.AddWebSocketService<Shop>("/shoplistener");
            wssv.Start();
        }
    }
}
