using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Main.global.interfaces;

namespace NettyBaseReloadedController.Main.global.managers
{
    class TickManager
    {
        public static short TICKS_PER_SECOND = 64;

        public List<ITick> Tickables = new List<ITick>();

        public async void Tick()
        {
            while (true)
            {
                foreach (var tickable in Tickables)
                {
                    tickable.Tick();
                }
                await Task.Delay(1000 / TICKS_PER_SECOND);
            }
        }

    }
}
