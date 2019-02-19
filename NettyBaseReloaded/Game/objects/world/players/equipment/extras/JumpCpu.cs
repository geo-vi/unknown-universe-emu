using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.player;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class JumpCpu : Extra
    {
        public override int Level => GetLevel();

        public JumpCpu(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
        }

        public int GetLevel()
        {
            switch (EquipmentItem.Item.Id)
            {
                case 70:
                    return 1;
                case 71:
                    return 2;
            }

            return 0;
        }

        public override void execute()
        {
            if (Player.State.IsOnHomeMap() &&
                (Player.Spacemap.Id == 1 || Player.Spacemap.Id == 5 || Player.Spacemap.Id == 9)) return;
            var station = Player.GetClosestStation(true);
            Player.MoveToMap(station.Item2, station.Item1, 0);
            Amount -= 1;
            Player.Controller.CPUs.Update(CPU.Types.JCPU);
            base.execute();
        }
    }
}
