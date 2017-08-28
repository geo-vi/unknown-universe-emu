    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using NettyBaseReloadedBrowser.Utils;

namespace NettyBaseReloadedBrowser.Game.netty.clientResponses
{
    abstract class IResponse : SimpleCommand
    {
        public abstract void execute();
    }
}
