using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Main.objects;

namespace Server.Utils
{
    class RandomInstance : ITick
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
        
        public int TickId { get; set; }
        
        /// <summary>
        /// Used to save the seed for the current instance.
        /// If instance is inactive it will get auto removed from the pool.
        /// </summary>
        public Random Random { get; private set; }

        public DateTime LastActiveAt = new DateTime();

        public int RandomsPerformed = 0;

        private RandomInstance()
        {
            Random = new Random();
            FixActivity();
        }

        public int GetId()
        {
            throw new NotImplementedException();
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
            RandomsPerformed++;
            if (RandomsPerformed > 1000)
            {
                Random = new Random();
                RandomsPerformed = 0;
            }
        }

        /* int methods */
        public int Next()
        {
            FixActivity();
            return Random.Next();
        }

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

        /// <summary>
        /// From 0.0 to 0.1
        /// </summary>
        /// <returns></returns>
        public double NextDouble()
        {
            FixActivity();
            return Random.NextDouble();
        }


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