using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Main.interfaces;
using System.Diagnostics;
using System.Threading;

namespace NettyBaseReloaded.Main.global_managers
{
    class TickManager
    {
        public static short TICKS_PER_SECOND = 64;

        /// <summary>
        /// ITick, Delay *TODO* 
        /// </summary>
        private ConcurrentDictionary<int, ITick> Tickables = new ConcurrentDictionary<int, ITick>();

        private int GetNextTickId()
        {
            using (var enumerator = Tickables.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return 0;

                var nextKeyInSequence = enumerator.Current.Key + 1;

                if (nextKeyInSequence < 1)
                    throw new InvalidOperationException("The dictionary contains keys less than 0");

                if (nextKeyInSequence != 1)
                    return 0;

                while (enumerator.MoveNext())
                {
                    var key = enumerator.Current.Key;
                    if (key > nextKeyInSequence)
                        return nextKeyInSequence;

                    ++nextKeyInSequence;
                }

                return nextKeyInSequence;
            }
        }
        
        public void Add(ITick tick, out int id)
        {
            id = -1;
            if (Tickables.Values.Contains(tick))
            {
                return;
            }

            id = GetNextTickId();
            Tickables.TryAdd(id, tick);
        }

        public void Remove(ITick tick)
        {
            ITick output;
            if (!Tickables.ContainsKey(tick.GetId()))
            {
                Console.WriteLine("Trying to remove nonexistent tick");
                return;
            }

            Tickables.TryRemove(tick.GetId(), out output);
        }

        public bool Exists(ITick tickable)
        {
            if (Tickables.Count == 0) return false;
            if (Tickables.ContainsKey(tickable.GetId())) return true;
            return false;
        }

        public async void Tick()
        {
            while (true)
            {
                foreach (var tickable in Tickables) { 
                    tickable.Value.Tick();
                }
                await Task.Delay(84);
            }
        }
    }
}
