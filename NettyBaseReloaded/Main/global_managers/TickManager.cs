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
using NettyBaseReloaded.Game.controllers;
using Task = System.Threading.Tasks.Task;

namespace NettyBaseReloaded.Main.global_managers
{
    class TickManager
    {
        public class PendingTick
        {
            public ITick Tick;
            public int PoolId;
            public PendingTick(ITick tick) => Tick = tick;
        }

        public static short TICKS_PER_SECOND = 64;

        /// <summary>
        /// ITick, Delay *TODO* 
        /// </summary>
        private List<ITick> Tickables = new List<ITick>();

        private List<PendingTick> PendingToBeAdded = new List<PendingTick>();
        private List<PendingTick> PendingRemoval = new List<PendingTick>();

        private List<ITick> NpcPool = new List<ITick>();

        public void Add(ITick tick)
        {           
            if (!Tickables.Contains(tick))
                PendingToBeAdded.Add(new PendingTick(tick));
        }

        public void Remove(ITick tick)
        {
            if (Tickables.Contains(tick))
                PendingRemoval.Remove(new PendingTick(tick));
        }

        public bool Exists(ITick tickable)
        {
            if (Tickables.Count == 0) return false;
            if (Tickables.Contains(tickable)) return true;
            return false;
        }

        public void InitiatePools()
        {
            var task = new Task(TickTickables, TaskCreationOptions.LongRunning);
            task.Start();
            task = new Task(TickNpcPool, TaskCreationOptions.LongRunning);
            task.Start();
        }
        
        private async void TickTickables()
        {
            while (true)
            {
                UpdateCollection(0);
                foreach (var tickable in Tickables)
                {
                    tickable.Tick();
                }
                await Task.Delay(84);
            }
        }

        private async void TickNpcPool()
        {
            while (true)
            {
                UpdateCollection(1);
                foreach (var npcPoolTick in NpcPool)
                {
                    npcPoolTick.Tick();
                }
                await Task.Delay(84);
            }
        }

        private void UpdateCollection(int poolId)
        {
            if (PendingToBeAdded.Count > 0)
            {
                if (poolId == 0)
                    Tickables.AddRange(PendingToBeAdded.ToArray().Where(x => x.PoolId == 0).Select(x => x.Tick));
                else if (poolId == 1) NpcPool.AddRange(PendingToBeAdded.ToArray().Where(x => x.PoolId == 1).Select(x => x.Tick));
                PendingRemoval.RemoveAll(x => x.PoolId == poolId);
            }
            if (PendingRemoval.Count > 0)
            {
                if (poolId == 0)
                    foreach (var tick in PendingRemoval.ToArray().Where(x => x.PoolId == 0))
                        Tickables.Remove(tick.Tick);
                else if (poolId == 1)
                    foreach (var tick in PendingRemoval.ToArray().Where(x => x.PoolId == 1))
                        NpcPool.Remove(tick.Tick); 
                PendingRemoval.RemoveAll(x => x.PoolId == poolId);
            }
        }

        public void AddToNpcPool(ITick npcPoolTick)
        {
            PendingToBeAdded.Add(new PendingTick(npcPoolTick) { PoolId = 1});
        }

        public void RemoveFromNpcPool(ITick npcPoolTick)
        {
            PendingRemoval.Add(new PendingTick(npcPoolTick) { PoolId = 1 });
        }
    }
}
