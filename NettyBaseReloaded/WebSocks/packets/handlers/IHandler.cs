using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.WebSocks.packets.handlers
{
    interface IHandler
    {
        void execute(WebSocketReceiver receiver, string[] packet);
    }
}
