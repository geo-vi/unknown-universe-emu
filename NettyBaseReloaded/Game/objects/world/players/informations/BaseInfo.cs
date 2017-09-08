using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    abstract class BaseInfo : PlayerBaseClass
    {
        internal int Value { get; set; }

        internal int SyncedValue { get; set; }

        public DateTime LastTimeSynced { get; set; }

        public BaseInfo(Player player) : base(player)
        {
            LastTimeSynced = DateTime.Now;
        }

        public abstract void Refresh();

        public abstract void Add(int amount);

        public abstract void Remove(int amount);

        public abstract void Set(int value);

        public int Get()
        {
            return Value;
        }
    }
}
