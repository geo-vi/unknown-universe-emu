using System;
using System.Collections.Concurrent;
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

        /// <summary>
        /// ITick, Delay *TODO* 
        /// </summary>
        private List<ITick> Tickables = new List<ITick>();

        private List<ITick> PendingToBeAdded = new List<ITick>();
        private List<ITick> PendingRemoval = new List<ITick>();

        public void Add(ITick tick)
        {
            PendingToBeAdded.Add(tick);
        }

        public void Remove(ITick tick)
        {
            PendingRemoval.Remove(tick);
        }

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
                UpdateCollection();
                foreach (var tick in Tickables) tick.Tick();
                await Task.Delay(1000 / TICKS_PER_SECOND);
            }
        }

        private void UpdateCollection()
        {
            Tickables.AddRange(PendingToBeAdded);
            PendingToBeAdded.Clear();
            foreach (var tick in PendingRemoval)
                Tickables.Remove(tick);
            PendingRemoval.Clear();
        }
    }
}
