using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    class Credits : BaseInfo
    {
        public Credits(Player player) : base(player)
        {

        }

        public override void Refresh()
        {
            World.DatabaseManager.LoadInfo(Player, this);
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
