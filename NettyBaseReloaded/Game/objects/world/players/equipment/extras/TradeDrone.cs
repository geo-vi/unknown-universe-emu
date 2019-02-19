using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class TradeDrone : Extra
    {
        public TradeDrone(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
        }

        public override void initiate()
        {
            var gameSession = Player.GetGameSession();
            if (gameSession == null) return;
            base.initiate();
        }
    }
}
