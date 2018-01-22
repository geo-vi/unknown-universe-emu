using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Uridium : BaseInfo
    {

        public Uridium(Player player) : base(player)
        {
            SqlName = "URIDIUM";
        }

        public override void Refresh()
        {
            World.DatabaseManager.LoadInfo(Player, this);
            Value = SyncedValue;
        }

        public override void Add(double amount)
        {
            World.DatabaseManager.UpdateInfo(Player, this, amount);
            Value = SyncedValue;
        }

        public override void Remove(double amount)
        {
            World.DatabaseManager.UpdateInfo(Player, this, -amount);
            Value = SyncedValue;
        }

        public override void Set(double value)
        {
            throw new NotImplementedException();
        }
    }
}
