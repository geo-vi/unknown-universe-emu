using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace NettyBaseReloaded.WebSocks
{
    class WebSocketListener
    {
        public static void InitiateListener()
        {
            var wssv = new WebSocketServer(666);
            wssv.AddWebSocketService<WebSocketReceiver>("/cmslistener");
            wssv.AddWebSocketService<WebSocketReceiver>("/external");
            wssv.Start();
        }
    }
}
