using System;
using System.Collections.Generic;
using NettyBaseReloaded.Helper.objects;
using NettyStatusBot.core;
using Newtonsoft.Json;

namespace NettyStatusBot.Networking.packets
{
    class InitPacket : PacketBase
    {
        public override string Header => "init";

        public override void readPacket(string packet)
        {
            var split = packet.Split('|');
            Global.ProcessId = Convert.ToInt32(split[1]);
            Global.ConnectedPlayers = JsonConvert.DeserializeObject<List<ConnectedPlayer>>(split[2]);
        }
    }
}
