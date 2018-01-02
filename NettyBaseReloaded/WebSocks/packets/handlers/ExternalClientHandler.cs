using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.WebSocks.objects;
using Newtonsoft.Json;

namespace NettyBaseReloaded.WebSocks.packets.handlers
{
    class ExternalClientHandler : IHandler
    {
        public void execute(WebSocketReceiver receiver, string[] packet)
        {
            switch (packet[1])
            {
                case "req":
                    switch (packet[2])
                    {
                        case "payload":
                            receiver.Send("payload|" + JsonConvert.SerializeObject(new Payload{Servers = Global.QueryManager.GetServers(), User = Global.QueryManager.GetUser(Convert.ToInt32(packet[3]))}));
                            break;
                    }
                    break;
            }
        }
    }
}
