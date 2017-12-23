using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Exp : BaseInfo
    {
        public Exp(Player player) : base(player)
        {
            
        }

        public override void Refresh()
        {
            World.DatabaseManager.LoadInfo(Player, this);
            Value = SyncedValue;
        }

        public override void Add(long amount)
        {
            World.DatabaseManager.UpdateInfo(Player, this, amount);
            Value = SyncedValue;
        }

        public override void Remove(long amount)
        {
            World.DatabaseManager.UpdateInfo(Player, this, -amount);
            Value = SyncedValue;
        }

        public override void Set(long value)
        {
            throw new NotImplementedException();
        }
    }
}
