using System;

namespace Server.Game.objects.entities.ships.items
{
    static class ItemMap
    {
        public static string[] LaserIds =
        {
            "ammunition_laser_lcb-10", "ammunition_laser_mcb-25", "ammunition_laser_mcb-50",
            "ammunition_laser_ucb-100", "ammunition_laser_sab-50", "ammunition_laser_cbo-100",
            "ammunition_laser_rsb-75", "ammunition_laser_job-100"
        };

        public static string[] RocketIds =
        {
            "ammunition_rocket_r-310", "ammunition_rocket_plt-2026", "ammunition_rocket_plt-2021",
            "ammunition_rocket_plt-3030", "ammunition_specialammo_pld-8", "ammunition_specialammo_dcr-250",
            "ammunition_specialammo_wiz-x", "ammunition_rocket_bdr-1211"
        };

        public static string[] RocketLauncherIds =
        {
            "equipment_weapon_rocketlauncher_not_present", "ammunition_rocketlauncher_hstrm-01",
            "ammunition_rocketlauncher_ubr-100",
            "ammunition_rocketlauncher_eco-10", "ammunition_rocketlauncher_sar-01",
            "ammunition_rocketlauncher_sar-02"
        };

        public static string[] SpecialItemsIds =
        {
            "ammunition_mine_smb-01", "equipment_extra_cpu_ish-01", "ammunition_specialammo_emp-01",
            "ammunition_firework_fwx-s", "ammunition_firework_fwx-m", "ammunition_firework_fwx-l",
            "ammunition_firework_ignite"
        };

        public static string[] MinesIds =
        {
            "ammunition_mine_acm-01", "ammunition_mine_empm-01", "ammunition_mine_sabm-01",
            "ammunition_mine_ddm-01", "ammunition_mine_slm-01", "ammunition_mine_im-01"
        };

        public static string[] CpusIds =
        {
            "equipment_extra_cpu_aim-01", "equipment_extra_cpu_aim-02", "equipment_extra_cpu_ajp-01",
            "equipment_extra_cpu_alb-x", "equipment_extra_cpu_anti-z1", "equipment_extra_cpu_anti-z1-xl",
            "equipment_extra_cpu_arol-x", "equipment_extra_cpu_cl04k-m", "equipment_extra_cpu_cl04k-xl",
            "equipment_extra_cpu_cl04k-xs", "equipment_extra_cpu_dr-01", "equipment_extra_cpu_dr-02",
            "equipment_extra_cpu_fb-x", "equipment_extra_cpu_jp-01",
            "equipment_extra_cpu_jp-02", "equipment_extra_cpu_min-t01", "equipment_extra_cpu_min-t02",
            "equipment_extra_cpu_nc-agb", "equipment_extra_cpu_nc-awb", "equipment_extra_cpu_nc-awl",
            "equipment_extra_cpu_nc-awr", "equipment_extra_cpu_nc-rrb", "equipment_extra_cpu_rb-x",
            "equipment_extra_cpu_rd-x", "equipment_extra_cpu_rllb-x", "equipment_extra_cpu_rok-t01",
            "equipment_extra_cpu_sle-01", "equipment_extra_cpu_sle-02", "equipment_extra_cpu_sle-03",
            "equipment_extra_cpu_sle-04", "equipment_extra_hmd-07", "equipment_extra_repbot_rep-1",
            "equipment_extra_repbot_rep-2", "equipment_extra_repbot_rep-3", "equipment_extra_repbot_rep-4",
            "equipment_extra_repbot_rep-s", "equipment_extra_cpu_nc-rrb-x",
            "equipment_extra_cpu_g3x-crgo-x"
        };

        public static string[] BuyNowIds =
        {
            "ammunition_laser_lcb-10", "ammunition_laser_mcb-25", "ammunition_laser_mcb-50",
            "ammunition_laser_sab-50", "ammunition_rocket_r-310", "ammunition_rocket_plt-2026",
            "ammunition_rocket_plt-2021", "ammunition_rocket_plt-3030"
        };

        public static string[] FormationIds =
        {
            "drone_formation_default", "drone_formation_f-01-tu",
            "drone_formation_f-02-ar", "drone_formation_f-03-la", "drone_formation_f-04-st",
            "drone_formation_f-05-pi",
            "drone_formation_f-06-da", "drone_formation_f-07-di", "drone_formation_f-08-ch",
            "drone_formation_f-09-mo",
            "drone_formation_f-10-cr", "drone_formation_f-11-he", "drone_formation_f-12-ba",
            "drone_formation_f-13-bt"
        };

        public static string[] TechIds =
        {
            "tech_backup-shields", "tech_battle-repair-bot", "tech_chain-impulse",
            "tech_energy-leech", "tech_precision-targeter"
        };

        public static string[] ShipAbilitiesIds =
        {
            "ability_admin-ultimate-cloaking", "ability_aegis_hp-repair",
            "ability_aegis_repair-pod", "ability_aegis_shield-repair", "ability_citadel_draw-fire",
            "ability_citadel_fortify", "ability_citadel_protection", "ability_citadel_travel", "ability_diminisher",
            "ability_lightning", "ability_sentinel", "ability_solace", "ability_spearhead_double-minimap",
            "ability_spearhead_jam-x", "ability_spearhead_target-marker", "ability_spearhead_ultimate-cloak",
            "ability_spectrum", "ability_venom"
        };

        public static bool IsSecondaryAmmunition(string lootId)
        {
            return lootId == LaserIds[6];
        }
    }
}