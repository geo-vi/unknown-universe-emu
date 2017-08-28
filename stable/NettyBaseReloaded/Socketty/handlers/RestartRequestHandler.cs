using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Socketty.handlers
{
    class RestartRequestHandler : IHandler
    {
        public void execute(string packet)
        {
            Process.Start("NettyRestarter.exe");
        }
    }
}
