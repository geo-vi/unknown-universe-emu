using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloadedBrowser.Game.netty.clientResponses
{
    class ShipInitializationCommand : IResponse
    {
        public const int ID = 26642;

        public override void execute()
        {
            readShort();
            var id = readInt();
            var name = readUTF();
            Debug.WriteLine("ID: {0}, Player name: {1}",id, name);

            Game.GameServer.Send(PacketBuilder.ShipInitializationCommand());
        }
    }
}
