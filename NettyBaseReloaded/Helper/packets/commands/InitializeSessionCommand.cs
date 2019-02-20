using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Helper.objects;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Helper.packets.commands
{
    class InitializeSessionCommand : ICommand
    {
        public override string Prefix => "init";

        public InitializeSessionCommand(List<ConnectedPlayer> connectedPlayers)
        {
            AddParam(Process.GetCurrentProcess().Id.ToString());
            AddParam(JsonConvert.SerializeObject(connectedPlayers));
        }
    }
}
