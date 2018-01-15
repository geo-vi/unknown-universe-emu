using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.events
{
    interface IEvent
    {
        void Tick();
        void Start();
        void Finish();
    }
}
