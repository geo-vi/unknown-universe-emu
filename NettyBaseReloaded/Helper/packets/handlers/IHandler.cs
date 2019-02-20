using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Helper.packets.requests;

namespace NettyBaseReloaded.Helper.packets.handlers
{
    interface IHandler
    {
        void Execute(HelperBrain brain, string[] packet);
    }
}
