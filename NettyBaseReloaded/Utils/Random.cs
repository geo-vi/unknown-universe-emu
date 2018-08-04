﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded
{
    public class RandomInstance : ITick
    {
        public static ConcurrentDictionary<object, RandomInstance> Instances = new ConcurrentDictionary<object, RandomInstance>();

        public static RandomInstance getInstance(object owner)
        {
            RandomInstance instance;
            if (Instances.TryGetValue(owner, out instance))
            {
                return instance;
            }
            instance = new RandomInstance();
            Instances.TryAdd(owner, instance);
            return instance;
        }
        /// <summary>
        /// Used to save the seed for the current instance.
        /// If instance is inactive it will get auto removed from the pool.
        /// </summary>
        public Random Random { get; }

        public DateTime LastActiveAt = new DateTime();

        private RandomInstance()
        {
            Random = new Random();
            FixActivity();
        }

        public void Tick()
        {
            foreach (var instance in Instances)
            {
                RandomInstance removedInstance;
                if (instance.Value.LastActiveAt.AddSeconds(5) < DateTime.Now)
                {
                    Instances.TryRemove(instance.Key, out removedInstance);
                }        
            }

            FixActivity();
        }

        #region Methods

        public void FixActivity()
        {
            LastActiveAt = DateTime.Now;
        }
        
        /* int methods */
        public int Next(int max)
        {
            FixActivity();
            return Random.Next(max);
        }

        public int Next(int v1, int v2)
        {
            FixActivity();
            return Random.Next(v1, v2);
        }
        /* int methods end */

        public void EndPool()
        {
            var instance = Instances.FirstOrDefault(x => x.Value == this);
            if (instance.Value != null)
            {
                RandomInstance removedInstance;
                Instances.TryRemove(instance.Key, out removedInstance);
            }
        }
        #endregion

    }
}
