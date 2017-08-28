using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Main.interfaces;
using System.Diagnostics;

namespace NettyBaseReloaded.Main.global_managers
{
    class TickManager
    {
        public static short TICKS_PER_SECOND = 512;

        public List<ITick> Tickables = new List<ITick>();

        public bool Exists(ITick tickable)
        {
            if (Tickables.Count == 0) return false;
            if (Tickables.Contains(tickable)) return true;
            return false;
        }

        public async void Tick()
        {
            while (true)
            {
                try
                {
                    if (Tickables.Count > 0)
                    {
                        foreach (var tickable in Tickables)
                        {
                            tickable.Tick();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Debug.WriteLine(e.StackTrace);
                }
                await Task.Delay(1000 / TICKS_PER_SECOND);
            }
        }
    }
}
