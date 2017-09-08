using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players.equipment;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Equipment : PlayerBaseClass
    {
        public Equipment(Player player) : base(player)
        {
        }

        public string GetConsumablesPacket()
        {
            bool rep = false;
            bool droneRep = false;
            bool ammoBuy = false;
            bool cloak = false;
            bool tradeDrone = false;
            bool smb = false;
            bool ish = false;
            bool aim = false;
            bool autoRocket = false;
            bool autoRocketLauncer = false;
            bool rocketBuy = false;
            bool jump = false;
            bool petRefuel = false;
            bool jumpToBase = false;

            var currConfig = Player.Hangar.Configurations[Player.CurrentConfig - 1];
            if (currConfig.Consumables != null &&
                currConfig.Consumables.Count > 0)
            {
                foreach (var item in currConfig.Consumables)
                {
                    var slotbarItem = Player.Settings.Slotbar._items[item.Value.LootId];
                    if (slotbarItem != null)
                    {
                        slotbarItem.CounterValue = item.Value.Amount;
                        slotbarItem.Visible = true;
                        if (Player.UsingNewClient)
                        {
                            World.StorageManager.GetGameSession(Player.Id)?.Client.Send(slotbarItem.ChangeStatus());
                        }
                    }

                    switch (item.Key)
                    {
                        case "equipment_extra_cpu_ajp-01":
                            jump = true;
                            break;
                        case "equipment_extra_repbot_rep-s":
                        case "equipment_extra_repbot_rep-1":
                        case "equipment_extra_repbot_rep-2":
                        case "equipment_extra_repbot_rep-3":
                        case "equipment_extra_repbot_rep-4":
                            rep = true;
                            break;
                        case "equipment_extra_cpu_smb-01":
                            smb = true;
                            break;
                        case "equipment_extra_cpu_ish-01":
                            ish = true;
                            break;
                        case "equipment_extra_cpu_aim-01":
                        case "equipment_extra_cpu_aim-02":
                            aim = true;
                            break;
                        case "equipment_extra_cpu_jp-01":
                        case "equipment_extra_cpu_jp-02":
                            jumpToBase = true;
                            break;
                        case "equipment_extra_cpu_cl04k-xl":
                        case "equipment_extra_cpu_cl04k-m":
                        case "equipment_extra_cpu_cl04k-xs":
                            cloak = true;
                            break;
                        case "equipment_extra_cpu_arol-x":
                            autoRocket = true;
                            break;
                        case "equipment_extra_cpu_rllb-x":
                            autoRocketLauncer = true;
                            break;
                        case "equipment_extra_cpu_dr-01":
                        case "equipment_extra_cpu_dr-02":
                            droneRep = true;
                            break;
                    }
                }
            }

            return Convert.ToInt32(droneRep) + "|0|" + Convert.ToInt32(jumpToBase) + "|" +
                   Convert.ToInt32(ammoBuy) + "|" + Convert.ToInt32(rep) + "|" + Convert.ToInt32(tradeDrone) +
                   "|0|" + Convert.ToInt32(smb) + "|" + Convert.ToInt32(ish) + "|0|" + Convert.ToInt32(aim) + "|" +
                   Convert.ToInt32(autoRocket) + "|" + Convert.ToInt32(cloak) + "|" +
                   Convert.ToInt32(autoRocketLauncer) + "|" + Convert.ToInt32(rocketBuy) + "|" +
                   Convert.ToInt32(jump) + "|" + Convert.ToInt32(petRefuel);

        }

        public string GetRobot()
        {
            return "equipment_extra_repbot_rep-4";
        }

        public int GetRobotLevel()
        {
            return 4;
        }

        public int LaserCount()
        {
            return Player.Hangar.Configurations[Player.CurrentConfig - 1].LaserCount;
        }
    }
}
