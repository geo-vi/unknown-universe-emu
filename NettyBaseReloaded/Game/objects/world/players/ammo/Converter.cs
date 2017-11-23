using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.players.settings;

namespace NettyBaseReloaded.Game.objects.world.players.ammo
{
    static class Converter
    {
        public static string AmmoToDbString(string lootId)
        {
            switch (lootId)
            {
                case "ammunition_laser_lcb-10":
                    return "LCB_10";
                case "ammunition_laser_mcb-25":
                    return "MCB_25";
                case "ammunition_laser_mcb-50":
                    return "MCB_50";
                case "ammunition_laser_ucb-100":
                    return "UCB_100";
                case "ammunition_laser_sab-50":
                    return "SAB_50";
                case "ammunition_laser_cbo-100":
                    return "CBO_100";
                case "ammunition_laser_rsb-75":
                    return "RSB_75";
                case "ammunition_laser_job-100":
                    return "JOB_100";
                case "ammunition_rocket_r-310":
                    return "R_310";
                case "ammunition_rocket_plt-2026":
                    return "PLT_2026";
                case "ammunition_rocket_plt-2021":
                    return "PLT_2021";
                case "ammunition_rocket_plt-3030":
                    return "PLT_3030";
                case "ammunition_specialammo_pld-8":
                    return "PLD_8";
                case "ammunition_specialammo_dcr-250":
                    return "DCR_250";
                case "ammunition_specialammo_wiz-x":
                    return "WIZ_X";
                case "ammunition_rocket_bdr-1211":
                    return "BDR_1211";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string AmmoTypeToString(short ammoType)
        {
            switch (ammoType)
            {
                case AmmunitionTypeModule.X1:
                    return Slotbar.Items.LaserIds[0];
                case AmmunitionTypeModule.X2:
                    return Slotbar.Items.LaserIds[1];
                case AmmunitionTypeModule.X3:
                    return Slotbar.Items.LaserIds[2];
                case AmmunitionTypeModule.X4:
                    return Slotbar.Items.LaserIds[3];
                case AmmunitionTypeModule.SAB:
                    return Slotbar.Items.LaserIds[4];
                case AmmunitionTypeModule.CBO:
                    return Slotbar.Items.LaserIds[5];
                case AmmunitionTypeModule.RSB:
                    return Slotbar.Items.LaserIds[6];
                case AmmunitionTypeModule.JOB100:
                    return Slotbar.Items.LaserIds[7];
                case AmmunitionTypeModule.R310:
                    return Slotbar.Items.RocketIds[0];
                case AmmunitionTypeModule.PLT2026:
                    return Slotbar.Items.RocketIds[1];
                case AmmunitionTypeModule.PLT2021:
                    return Slotbar.Items.RocketIds[2];
                case AmmunitionTypeModule.PLT3030:
                    return Slotbar.Items.RocketIds[3];
                case AmmunitionTypeModule.PLASMA:
                    return Slotbar.Items.RocketIds[4];
                case AmmunitionTypeModule.DECELERATION:
                    return Slotbar.Items.RocketIds[5];
                case AmmunitionTypeModule.WIZARD:
                    return Slotbar.Items.RocketIds[6];
                case AmmunitionTypeModule.BDR1211:
                    return Slotbar.Items.RocketIds[7];
                default:
                    throw new NotImplementedException();
            }
        }

        public static AmmunitionTypeModule ToAmmoType(string lootId)
        {
            switch (lootId)
            {
                case "ammunition_laser_lcb-10":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.X1);
                case "ammunition_laser_mcb-25":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.X2);
                case "ammunition_laser_mcb-50":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.X3);
                case "ammunition_laser_ucb-100":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.X4);
                case "ammunition_laser_sab-50":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.SAB);
                case "ammunition_laser_cbo-100":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.CBO);
                case "ammunition_laser_rsb-75":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.RSB);
                case "ammunition_laser_job-100":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.JOB100);
                case "ammunition_rocket_r-310":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.R310);
                case "ammunition_rocket_plt-2026":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.PLT2026);
                case "ammunition_rocket_plt-2021":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.PLT2021);
                case "ammunition_rocket_plt-3030":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.PLT3030);
                case "ammunition_specialammo_pld-8":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.PLASMA);
                case "ammunition_specialammo_dcr-250":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.DECELERATION);
                case "ammunition_specialammo_wiz-x":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.WIZARD);
                case "ammunition_rocket_bdr-1211":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.BDR1211);
                default:
                    throw new NotImplementedException();
            }
        }

        public static int GetLootAmmoId(string lootId)
        {
            switch (lootId)
            {
                case "ammunition_laser_lcb-10":
                case "ammunition_rocket_r-310":
                    return 1;
                case "ammunition_laser_mcb-25":
                case "ammunition_rocket_plt-2026":
                    return 2;
                case "ammunition_laser_mcb-50":
                case "ammunition_rocket_plt-2021":
                    return 3;
                case "ammunition_laser_ucb-100":
                case "ammunition_rocket_plt-3030":
                    return 4;
                case "ammunition_laser_sab-50":
                case "ammunition_rocket_bdr-1211":
                    return 5;
                case "ammunition_laser_rsb-75":
                    return 6;
                case "ammunition_laser_cbo-100":
                case "ammunition_specialammo_pld-8":
                    return 7;
                case "ammunition_specialammo_dcr-250":
                    return 8;
                case "ammunition_laser_job-100":
                case "ammunition_specialammo_wiz-x":
                    return 9;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
