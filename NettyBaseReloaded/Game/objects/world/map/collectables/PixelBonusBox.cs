using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class PixelBonusBox : BonusBox
    {
        public PixelBonusBox(int id, string hash, Types type, Vector pos, Spacemap map, Vector[] limits, bool respawning = false, bool isHoneyBox = false) : base(id, hash, type, pos, map, limits, respawning, isHoneyBox)
        {
        }

        public override void Collect(Character character)
        {
            base.Collect(character);
            if (character is Player player)
            {
                player.Statistics.CollectBox(true);
            }
        }
    }
}
