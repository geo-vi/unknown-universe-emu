using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Equipment : PlayerBaseClass
    {
        #region Hangars
        public int ActiveHangar = 0;

        public Dictionary<int, Hangar> Hangars { get; set; }
        #endregion

        public Dictionary<int, Module> Modules = new Dictionary<int, Module>();

        public bool ModuleEquipping = false;

        public List<DroneFormation> OwnedDroneFormations = new List<DroneFormation>();

        public Equipment(Player player) : base(player)
        {
            RefreshHangars();
            //Modules = World.DatabaseManager.LoadPlayerModules(player);
            OwnedDroneFormations = World.DatabaseManager.LoadDroneFormations(player);
            //AddModules();
        }

        private void AddModules()
        {
            Modules.Add(0, new Module(Module.Types.LASER_HIGH_RANGE, new Item(0, "", 0), false));
            Modules.Add(1, new Module(Module.Types.DAMAGE_BOOSTER, new Item(1, "", 0), false));
            Modules.Add(2, new Module(Module.Types.ROCKET_MID_ACCURACY, new Item(2, "", 0), false));
            Modules.Add(3, new Module(Module.Types.HULL, new Item(3, "", 0), false));
            Modules.Add(4, new Module(Module.Types.DEFLECTOR, new Item(4, "", 0), false));
            Modules.Add(5, new Module(Module.Types.DAMAGE_BOOSTER, new Item(5, "", 0), false));
            Modules.Add(6, new Module(Module.Types.EXPERIENCE_BOOSTER, new Item(6, "", 0), false));
            Modules.Add(7, new Module(Module.Types.REPAIR, new Item(7, "", 0), false));
            Modules.Add(8, new Module(Module.Types.ROCKET_LOW_ACCURACY, new Item(8, "", 0), false));
            Modules.Add(9, new Module(Module.Types.LASER_LOW_RANGE, new Item(9, "", 0), false));
            Modules.Add(10, new Module(Module.Types.HONOR_BOOSTER, new Item(10, "", 0), true));
            Modules.Add(12, new Module(Module.Types.ROCKET_MID_ACCURACY, new Item(12, "", 0), false));
            Modules.Add(11, new Module(Module.Types.ROCKET_LOW_ACCURACY, new Item(11, "", 0), false));
        }

        public void RefreshHangars()
        {
            Hangars = World.DatabaseManager.LoadHangars(Player);
            for (int hangarId = 0; hangarId < Hangars.Count; hangarId++)
            {
                if (Hangars[hangarId].Active)
                {
                    ActiveHangar = hangarId;
                    break;
                }
            }
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
            bool rokTurbo = false;

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
                        case "equipment_extra_cpu_rok-t01":
                            rokTurbo = true;
                            break;
                    }
                }
            }

            return Convert.ToInt32(droneRep) + "|1|" + Convert.ToInt32(jumpToBase) + "|" +
                   Convert.ToInt32(ammoBuy) + "|" + Convert.ToInt32(rep) + "|" + Convert.ToInt32(tradeDrone) +
                   "|1|" + Convert.ToInt32(smb) + "|" + Convert.ToInt32(ish) + "|1|" + Convert.ToInt32(aim) + "|" +
                   Convert.ToInt32(autoRocket) + "|" + Convert.ToInt32(cloak) + "|" +
                   Convert.ToInt32(autoRocketLauncer) + "|" + Convert.ToInt32(rocketBuy) + "|" +
                   Convert.ToInt32(jump) + "|" + Convert.ToInt32(petRefuel);

        }

        public string GetRobot()
        {
            if (Player.Extras.Any(x => x.Value is Robot))
                return Player.Extras.FirstOrDefault(x => x.Value is Robot).Key;
            return "";
        }

        public int GetRobotLevel()
        {
            return 4;
        }

        public int LaserCount()
        {
            return Player.Hangar.Configurations[Player.CurrentConfig - 1].LaserCount;
        }

        public int LaserTypes()
        {
            return Player.Hangar.Configurations[Player.CurrentConfig - 1].LaserTypes;
        }
    }
}
