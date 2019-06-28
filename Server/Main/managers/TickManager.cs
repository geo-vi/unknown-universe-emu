using Server.Main.objects;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.Main.managers
{
    class TickManager
    {
        /// <summary>
        /// ITick, Delay *TODO* 
        /// </summary>
        private ConcurrentDictionary<int, ITick> Tickables = new ConcurrentDictionary<int, ITick>();

        private int GetNextTickId()
        {
            var i = 0;
            while (true)
            {
                if (Tickables.ContainsKey(i))
                    i++;
                else return i;
            }
        }

        public void Add(ITick tick, out int id)
        {
            id = -1;
            if (Tickables.ContainsKey(id) || tick == null)
            {
                return;
            }

            id = GetNextTickId();
            Tickables.TryAdd(id, tick);
        }

        public void Remove(ITick tick)
        {
            ITick output;
            if (!Tickables.ContainsKey(tick.TickId))
            {
                return;
            }

            Tickables.TryRemove(tick.TickId, out output);
        }

        public bool Exists(ITick tickable)
        {
            if (Tickables.Count == 0) return false;
            if (Tickables.ContainsKey(tickable.TickId)) return true;
            return false;
        }

        public double LastTickMS = 0;
        public void Tick()
        {
            var t1 = DateTime.Now;
            var t2 = DateTime.Now;
            while (true)
            {

                t1 = DateTime.Now;
                ITick current = null;
                try
                {
                    Parallel.ForEach(Tickables, (tickable) =>
                    {
                        current = tickable.Value;
                        tickable.Value.Tick();
                    });
                }

                catch (Exception e)
                {
                    if (current != null)
                    {
                        var id = current.TickId;
                        Console.WriteLine("Error at tick " + id);
                        Tickables.TryRemove(id, out _);
                    }

                    Console.WriteLine("Caught exception on Tick");
                    Console.WriteLine(e);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                t2 = DateTime.Now;
                LastTickMS = (t2 - t1).TotalMilliseconds;
            }
        }
    }
}
