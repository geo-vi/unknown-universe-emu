using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using AmmunitionTypeModule = NettyBaseReloaded.Game.netty.commands.old_client.AmmunitionTypeModule;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world.players.settings.slotbars;

namespace NettyBaseReloaded.Game.objects.world.players.settings
{
    class Slotbar : PlayerBaseClass
    {
        public class Items
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
                "equipment_weapon_rocketlauncher_not_present","ammunition_rocketlauncher_hstrm-01",
                "ammunition_rocketlauncher_ubr-100",
                "ammunition_rocketlauncher_eco-10", "ammunition_rocketlauncher_sar-01",
                "ammunition_rocketlauncher_sar-02"
            };

            public static string[] SpecialItemsIds =
            {
                "ammunition_mine_smb-01", "equipment_extra_cpu_ish-01", "ammunition_specialammo_emp-01",
                "ammunition_firework_fwx-s", "ammunition_firework_fwx-m", "ammunition_firework_fwx-l", "ammunition_firework_ignite"
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
        }

        public List<SlotbarQuickslotItem> QuickslotItems = new List<SlotbarQuickslotItem>();
        public List<SlotbarQuickslotItem> PremiumQuickslotItems = new List<SlotbarQuickslotItem>();

        public Dictionary<string, SlotbarItem> _items = new Dictionary<string, SlotbarItem>();

        public Slotbar(Player player) : base(player)
        {
        }

        public List<SlotbarCategoryModule> GetCategories()
        {
            var counterValue = 0;

            var categories = new List<SlotbarCategoryModule>();
            var items = new List<SlotbarCategoryItemModule>();

            //LASERS
            var maxCounter = 1000;
            foreach (var itemId in Items.LaserIds)
            {
                if (!Player.Information.Ammunitions.ContainsKey(itemId) || itemId == "ammunition_laser_cbo-100" || itemId == "ammunition_laser_job-100") continue;
                var ammo = Player.Information.Ammunitions[itemId];

                var item = new LaserItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.CounterValue = ammo.Get();

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("lasers", items));

            //ROCKETS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 200;
            foreach (var itemId in Items.RocketIds)
            {
                if (!Player.Information.Ammunitions.ContainsKey(itemId)) continue;
                var ammo = Player.Information.Ammunitions[itemId];

                counterValue = ammo.Get();

                var item = new RocketItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("rockets", items));

            //ROCKET LAUNCHER
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 200;

            foreach (var itemId in Items.RocketLauncherIds)
            {
                if (!Player.Information.Ammunitions.ContainsKey(itemId)) continue;
                var ammo = Player.Information.Ammunitions[itemId];
                counterValue = ammo.Get();

                var item = new RocketLauncherItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("rocket_launchers", items));

            //SPECIAL ITEMS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 100;
            foreach (var itemId in Items.SpecialItemsIds)
            {
                var item = new SpecialItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.CounterValue = 100;

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("special_items", items));

            //MINES
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 100;
            foreach (var itemId in Items.MinesIds)
            {
                var item = new MineItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("mines", items));

            //CPUS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 100;
            foreach (var itemId in Items.CpusIds)
            {
                var item = new CpuItem(
                    itemId,
                    counterValue,
                    maxCounter,
                    null,
                    0,
                    false,
                    true
                );

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("cpus", items));

            //BUY NOW
            //items = new List<SlotbarCategoryItemModule>();
            //maxCounter = 0;
            //foreach (var itemId in Items.BuyNowIds)
            //{
            //    var item = new BuyItem(
            //        itemId,
            //        counterValue,
            //        maxCounter
            //    );

            //    item.Create();
            //    items.Add(item.Object);
            //    _items[item.ClickedId] = item;
            //}
            //categories.Add(new SlotbarCategoryModule("buy_now", items));

            //TECH ITEMS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 99;
            foreach (var itemId in Items.TechIds)
            {
                var item = new TechItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.Create();
                item.CounterValue = 99;
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("tech_items", items));

            //SHIP ABILITIES
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 0;
            foreach (var itemId in Items.ShipAbilitiesIds)
            {
                var item = new ShipAbilityItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("ship_abilities", items));

            //DRONE FORMATION
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 0;
            foreach (var itemId in Items.FormationIds)
            {
                var item = new FormationItem(
                    itemId,
                    counterValue,
                    maxCounter
                );

                item.Create();
                items.Add(item.Object);
                _items[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("drone_formations", items));
            
            return categories;
        }

        public void HideMenu(MenuButtons btn)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|MV|HM|" + (int) btn);
            }
        }

        public void ShowMenu(MenuButtons btn)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|MV|SM|" + (int)btn);
            }
        }

        public void DisableMenu(MenuButtons btn)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|MBA|DB|" + (int) btn);
            }
        }

        public void EnableMenu(MenuButtons btn)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|MBA|EB|" + (int)btn);
            }
        }

        public void HideButton(Buttons btn)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|B|HB|" + (int)btn);
            }
        }

        public void HideFlash(Buttons btn)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|B|HF|-1|-1|" + (int)btn);
            }
        }

        public void ShowFlash(Buttons btn, bool pointer, int times)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|B|SF|" + times +"|"+ Convert.ToInt32(pointer) + "|" + (int)btn);
            }
        }

        public void ShowButton(Buttons btn)
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.LegacyModule(session, "0|UI|B|SB|" + (int)btn);
            }
        }
    }
}
