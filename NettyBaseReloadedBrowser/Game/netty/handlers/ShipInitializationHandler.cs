using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedBrowser.Utils;

namespace NettyBaseReloadedBrowser.Game.netty.handlers
{
    class ShipInitializationHandler : IHandler
    {
        public void execute(ByteParser parser)
        {
            Debug.WriteLine(Game.GameClient);
            //Game.GameClient.Send(clientRequests.VersionRequest.write(250, "g5q17h4o8e951dfffcsb7mb4n1"));
        }
    }
}
