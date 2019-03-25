using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Main.interfaces
{
    interface ITick
    {
        int GetId();
        void Tick();
    }
}
