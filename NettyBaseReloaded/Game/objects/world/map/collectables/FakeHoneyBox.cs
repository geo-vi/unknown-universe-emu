using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class FakeHoneyBox : Collectable
    {
        public FakeHoneyBox(string hash) : base(0, hash, Types.BONUS_BOX, new Vector(0,0), World.StorageManager.Spacemaps[255], null, true)
        {
        }

        protected override void Reward(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
