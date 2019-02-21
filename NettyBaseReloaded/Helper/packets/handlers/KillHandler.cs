using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Helper.packets.handlers
{
    class KillHandler : IHandler
    {
        public void Execute(HelperBrain brain, string[] packet)
        {
            Program.Exit();
        }
    }
}
