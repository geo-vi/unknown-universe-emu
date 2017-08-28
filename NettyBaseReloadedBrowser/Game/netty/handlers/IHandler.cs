using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedBrowser.Utils;

namespace NettyBaseReloadedBrowser.Game.netty.handlers
{
    interface IHandler
    {
        void execute(ByteParser parser);
    }
}
