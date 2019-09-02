using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.WebSocks.packets.handlers
{
    class MapActivatedHandler : IHandler
    {
        public void execute(WebSocketReceiver receiver, string[] packet)
        {
            try
            {
                var userId = packet[1];
                
            }
            catch { }
        }
    }
}
