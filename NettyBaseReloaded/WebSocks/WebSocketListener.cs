using System;
using System.Collections.Generic;
using System.IO;
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
            var wssv = new WebSocketServer(666) { KeepClean = true, ReuseAddress = true};
            wssv.Log.Level = WebSocketSharp.LogLevel.Info;
            wssv.Log.File = Directory.GetCurrentDirectory() + "/wsslog.txt";
            wssv.AddWebSocketService("/cmslistener", () => new WebSocketReceiver { IgnoreExtensions = true });
            wssv.Start();
        }
    }
}
