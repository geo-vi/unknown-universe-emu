using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.WebSocks.packets.handlers
{
    class ClanHandler : IHandler
    {
        public void execute(WebSocketReceiver receiver, string[] packet)
        {
            try
            {
                var userId = int.Parse(packet[1]);
                switch (packet[2])
                {
                    case "new_create":
                        break;
                    case "update_members":
                        break;
                    case "left_clan":
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
