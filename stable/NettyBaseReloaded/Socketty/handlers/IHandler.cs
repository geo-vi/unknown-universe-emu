using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Socketty.handlers
{
    interface IHandler
    {
        void execute(string packet);
    }
}
