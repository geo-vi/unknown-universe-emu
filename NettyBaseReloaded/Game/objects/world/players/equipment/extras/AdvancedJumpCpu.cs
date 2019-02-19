using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class AdvancedJumpCpu : Extra
    {
        public AdvancedJumpCpu(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
        }

        public override void execute()
        {
        }

        public override void initiate()
        {
            List<int> disabledMaps = new List<int>{ 91, 92, 93, 29};
            List<int> lockedMaps = new List<int>{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28};
            List<int> notEnoughResources; //-1
            var openMaps = World.PortalSystemManager.GetOpenMapsList(Player);

            lockedMaps.RemoveAll(x => openMaps.Contains(x));

            notEnoughResources = CalculateAvailableMaps(ref openMaps);

            Packet.Builder.LegacyModule(Player.GetGameSession(), "0|A|JCPU|I|-2|" + String.Join(";", lockedMaps) + ";|-1|" + String.Join(";", notEnoughResources) + ";|0|" + String.Join(";", openMaps) + ";");
        }

        public List<int> CalculateAvailableMaps(ref List<int> openMaps)
        {
            List<int> notAvailable = new List<int>();
            foreach (var openMap in openMaps)
            {
                var distance = World.PortalSystemManager.CalculateDistance(Player.Spacemap.Id, openMap);
                if (Player.Information.Vouchers <= 0 && Player.Information.Uridium.Get() < distance * 50)
                    notAvailable.Add(openMap);
            }

            openMaps.RemoveAll(x => notAvailable.Contains(x));
            return notAvailable;
        }

    }
}
