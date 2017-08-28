using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.npc
{
    interface INpc
    {
        void Tick();
        void Active();
        void Inactive();
        void Paused();
        void Exit();
    }
}
