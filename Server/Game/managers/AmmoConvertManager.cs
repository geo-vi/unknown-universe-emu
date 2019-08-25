using System;
using Server.Game.netty.commands.old_client;
using Server.Game.objects.entities.players;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;

namespace Server.Game.managers
{
    static class AmmoConvertManager
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
                case "ammunition_rocketlauncher_hstrm-01":
                    return "HSTRM_01";
                case "ammunition_rocketlauncher_ubr-100":
                    return "UBR_100";
                case "ammunition_rocketlauncher_eco-10":
                    return "ECO_10";
                case "ammunition_rocketlauncher_sar-01":
                    return "SAR_01";
                case "ammunition_rocketlauncher_sar-02":
                    return "SAR_02";
                case "ammunition_mine_acm-01":
                    return "ACM_01";
                case "ammunition_mine_smb-01":
                    return "SMB_01";
                case "equipment_extra_cpu_ish-01":
                    return "ISH_01";
                case "ammunition_specialammo_emp-01":
                    return "EMP_01";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string AmmoTypeToString(short ammoType)
        {
            switch (ammoType)
            {
                case AmmunitionTypeModule.X1:
                    return ItemMap.LaserIds[0];
                case AmmunitionTypeModule.X2:
                    return ItemMap.LaserIds[1];
                case AmmunitionTypeModule.X3:
                    return ItemMap.LaserIds[2];
                case AmmunitionTypeModule.X4:
                    return ItemMap.LaserIds[3];
                case AmmunitionTypeModule.SAB:
                    return ItemMap.LaserIds[4];
                case AmmunitionTypeModule.CBO:
                    return ItemMap.LaserIds[5];
                case AmmunitionTypeModule.RSB:
                    return ItemMap.LaserIds[6];
                case AmmunitionTypeModule.JOB100:
                    return ItemMap.LaserIds[7];
                case AmmunitionTypeModule.R310:
                    return ItemMap.RocketIds[0];
                case AmmunitionTypeModule.PLT2026:
                    return ItemMap.RocketIds[1];
                case AmmunitionTypeModule.PLT2021:
                    return ItemMap.RocketIds[2];
                case AmmunitionTypeModule.PLT3030:
                    return ItemMap.RocketIds[3];
                case AmmunitionTypeModule.PLASMA:
                    return ItemMap.RocketIds[4];
                case AmmunitionTypeModule.DECELERATION:
                    return ItemMap.RocketIds[5];
                case AmmunitionTypeModule.WIZARD:
                    return ItemMap.RocketIds[6];
                case AmmunitionTypeModule.BDR1211:
                    return ItemMap.RocketIds[7];
                case AmmunitionTypeModule.HELLSTORM:
                    return ItemMap.RocketLauncherIds[1];
                case AmmunitionTypeModule.UBER_ROCKET:
                    return ItemMap.RocketLauncherIds[2];
                case AmmunitionTypeModule.ECO_ROCKET:
                    return ItemMap.RocketLauncherIds[3];
                case AmmunitionTypeModule.SAR01:
                    return ItemMap.RocketLauncherIds[4];
                case AmmunitionTypeModule.SAR02:
                    return ItemMap.RocketLauncherIds[5];
                case AmmunitionTypeModule.MINE:
                    return ItemMap.MinesIds[0];
                case AmmunitionTypeModule.SMARTBOMB:
                    return ItemMap.SpecialItemsIds[0];
                case AmmunitionTypeModule.INSTANT_SHIELD:
                    return ItemMap.SpecialItemsIds[1];
                case AmmunitionTypeModule.EMP:
                    return ItemMap.SpecialItemsIds[2];
                default:
                    return "";
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
                case "ammunition_rocketlauncher_hstrm-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.HELLSTORM);
                case "ammunition_rocketlauncher_ubr-100":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.UBER_ROCKET);
                case "ammunition_rocketlauncher_eco-10":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.ECO_ROCKET);
                case "ammunition_rocketlauncher_sar-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.SAR01);
                case "ammunition_rocketlauncher_sar-02":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.SAR02);
                case "ammunition_mine_acm-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.MINE);
                case "ammunition_mine_smb-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.SMARTBOMB);
                case "equipment_extra_cpu_ish-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.INSTANT_SHIELD);
                case "ammunition_specialammo_emp-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.EMP);
                case "ammunition_mine_empm-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.MINE_EMP);
                case "ammunition_mine_slm-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.MINE_SL);
                case "ammunition_mine_ddm-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.MINE_DD);
                case "ammunition_mine_sabm-01":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.MINE_SAB);
                case "ammunition_firework_fwx-s":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.FIREWORK_1);
                case "ammunition_firework_fwx-m":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.FIREWORK_2);
                case "ammunition_firework_fwx-l":
                    return new AmmunitionTypeModule(AmmunitionTypeModule.FIREWORK_3);
                default:
                    return new AmmunitionTypeModule(AmmunitionTypeModule.BATTERY);
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
                case "ammunition_specialammo_pld-8":
                    return 5;
                case "ammunition_laser_rsb-75":
                case "ammunition_specialammo_wiz-x":
                    return 6;
                case "ammunition_laser_cbo-100":
                case "ammunition_rocketlauncher_hstrm-01":
                    return 7;
                case "ammunition_rocketlauncher_ubr-100":
                    return 8;
                case "ammunition_laser_job-100":
                case "ammunition_rocketlauncher_eco-10":
                    return 9;
                case "ammunition_specialammo_dcr-250":
                    return 10;
                case "ammunition_rocket_bdr-1211":
                    return 11;
                case "ammunition_rocketlauncher_sar-01":
                    return 12;
                case "ammunition_rocketlauncher_sar-02":
                    return 13;
                default:
                    return 0;
            }
        }

        public static string GetLaserLootId(int ammoId)
        {
            switch (ammoId)
            {
                case 1:
                    return "ammunition_laser_lcb-10";
                case 2:
                    return "ammunition_laser_mcb-25";
                case 3:
                    return "ammunition_laser_mcb-50";
                case 4:
                    return "ammunition_laser_ucb-100";
                case 5:
                    return "ammunition_laser_sab-50";
                case 6:
                    return "ammunition_laser_rsb-75";
                case 7:
                    return "ammunition_laser_cbo-100";
                case 9:
                    return "ammunition_laser_job-100";
                default:
                    return "";
            }
        }
        
        public static string GetRocketLootId(int ammoId)
        {
            switch (ammoId)
            {
                case 1:
                    return "ammunition_rocket_r-310";
                case 2:
                    return "ammunition_rocket_plt-2026";
                case 3:
                    return "ammunition_rocket_plt-2021";
                case 4:
                    return "ammunition_rocket_plt-3030";
                case 5:
                    return "ammunition_specialammo_pld-8";
                case 6:
                    return "ammunition_specialammo_wiz-x";
                case 7:
                    return "ammunition_rocketlauncher_hstrm-01";
                case 8:
                    return "ammunition_rocketlauncher_ubr-100";
                case 9:
                    return "ammunition_rocketlauncher_eco-10";
                case 10:
                    return "ammunition_specialammo_dcr-250";
                case 11:
                    return "ammunition_rocket_bdr-1211";
                case 12:
                    return "ammunition_rocketlauncher_sar-01";
                case 13:
                    return "ammunition_rocketlauncher_sar-02";
                default:
                    return "";
            }
        }

    }
}