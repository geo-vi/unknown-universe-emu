using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.WebSocks.objects;

namespace NettyBaseReloaded.WebSocks.packets.handlers
{
    class AdminPanelHandler : IHandler
    {
        public void execute(WebSocketReceiver receiver, string[] packet)
        {
            try
            {
                switch (packet[1])
                {
                    case "auth":
                        var userId = 0;
                        var playerId = 0;
                        if (int.TryParse(packet[2], out userId) && int.TryParse(packet[3], out playerId))
                        {
                            var admin = new Admin(userId, playerId, packet[4], DateTime.Now);
                            admin.BeginAuth();
                            //todo...
                        }
                        break;
                    case "box":
                        break;
                    case "object":
                        break;
                    case "resource":
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
