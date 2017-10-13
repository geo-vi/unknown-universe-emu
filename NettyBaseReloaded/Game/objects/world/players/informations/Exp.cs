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
            if (SyncedValue != Value)
            {
                Value = SyncedValue + Value;
            }
            else Value = SyncedValue;
        }

        public override void Add(int amount)
        {
            throw new NotImplementedException();
        }

        public override void Remove(int amount)
        {
            throw new NotImplementedException();
        }

        public override void Set(int value)
        {
            throw new NotImplementedException();
        }
    }
}
