using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                            var servers = Global.QueryManager.GetServers();
                            var user = Global.QueryManager.GetUser(Convert.ToInt32(packet[3]));
                            var payload = new Payload{Servers = servers, User = user};
                            receiver.Send("payload|" + JsonConvert.SerializeObject(payload));
                            break;
                    }
                    break;
            }
        }
    }
}
