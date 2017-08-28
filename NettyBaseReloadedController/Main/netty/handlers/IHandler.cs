using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Networking;
using NettyBaseReloadedController.Utils;

namespace NettyBaseReloadedController.Main.netty.handlers
{
    interface IHandler
    {
        void execute(ControllerClient gameSession, ByteParser parser);
    }
}
