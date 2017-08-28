using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::NettyBaseReloadedController.Main.global;
using NettyBaseReloadedController.Networking;
using NettyBaseReloadedController.Utils;

namespace NettyBaseReloadedController.Main.netty.handlers
{
    class LoginCommandHandler : IHandler
    {
        public void execute(ControllerClient client, ByteParser parser)
        {
            var ready = parser.Boolean();

            if (ready) Controller.Global.STATE = Global.STATE_LOADED;
        }
    }
}
