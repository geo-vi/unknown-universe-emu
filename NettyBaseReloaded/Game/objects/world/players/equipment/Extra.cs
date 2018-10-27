using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    abstract class Extra : Item
    {
        protected Player Player { get; set; }

        public bool Active = false;

        protected Extra(Player player, int itemId, string lootId, int amount) : base(itemId, lootId, amount)
        {
            Player = player;
        }

        public virtual void initiate()
        {

        }

        public virtual void execute()
        {
            var cpus = Player.Settings.OldClientShipSettingsCommand.activeCpus;
            if (cpus != null)
            {
                if (!cpus.Contains(LootId))
                {
                    cpus.Add(LootId);
                }
                else
                {
                    cpus.Remove(LootId);
                }
            }
        }

        public static Dictionary<string, Extra> LoadExtras(Player player, Dictionary<string, Item> consumables)
        {
            var extras = new Dictionary<string, Extra>();
            foreach (var consumable in consumables)
            {
                switch (consumable.Key)
                {
                    case "equipment_extra_cpu_ajp-01":
                        extras.Add(consumable.Key, new AdvancedJumpCpu(player, consumable.Value.Id, consumable.Value.LootId,
                            consumable.Value.Amount));
                        break;
                    case "equipment_extra_cpu_rok-t01":
                        extras.Add(consumable.Key,
                            new Turbo(player, consumable.Value.Id, consumable.Value.LootId,
                                consumable.Value.Amount));
                        break;
                    case "equipment_extra_repbot_rep-s":
                    case "equipment_extra_repbot_rep-1":
                    case "equipment_extra_repbot_rep-2":
                    case "equipment_extra_repbot_rep-3":
                    case "equipment_extra_repbot_rep-4":
                        extras.Add(consumable.Key,
                            new Robot(player, consumable.Value.Id, consumable.Value.LootId,
                                consumable.Value.Amount));
                        break;
                    case "equipment_extra_cpu_smb-01":
                        // TODO: add Smartbomb
                        break;
                    case "equipment_extra_cpu_ish-01":
                        // TODO: Add ISH
                        break;
                    case "equipment_extra_cpu_aim-01":
                    case "equipment_extra_cpu_aim-02":
                        // TODO: Add aim cpu
                        break;
                    case "equipment_extra_cpu_jp-01":
                    case "equipment_extra_cpu_jp-02":
                        // TODO: Add jump2base
                        break;
                    case "equipment_extra_cpu_cl04k-xl":
                    case "equipment_extra_cpu_cl04k-m":
                    case "equipment_extra_cpu_cl04k-xs":
                        extras.Add(consumable.Key,
                            new Cloak(player, consumable.Value.Id, consumable.Value.LootId,
                                consumable.Value.Amount));
                        break;
                    case "equipment_extra_cpu_arol-x":
                        extras.Add(consumable.Key, new AutoRocket(player, consumable.Value.Id, consumable.Value.LootId,
                            consumable.Value.Amount));
                        break;
                    case "equipment_extra_cpu_rllb-x":
                        extras.Add(consumable.Key, new AutoRocketLauncher(player, consumable.Value.Id,
                            consumable.Value.LootId,
                            consumable.Value.Amount));
                        break;
                    case "equipment_extra_cpu_dr-01":
                    case "equipment_extra_cpu_dr-02":
                        // TODO: add drone rep
                        break;
                    case "equipment_extra_cpu_g3x-crgo-x":
                        extras.Add("equipment_extra_cpu_g3x-crgo-x", new CargoXtender(player, consumable.Value.Id,
                            consumable.Value.LootId,
                            consumable.Value.Amount));
                        break;
                }
            }


            return extras;
        }

        public static Dictionary<string, Item> ToItems(Dictionary<string, Extra> extras)
        {
            Dictionary<string,Item> items = new Dictionary<string, Item>();
            foreach (var extra in extras)
            {
                items.Add(extra.Key, new Item(extra.Value.Id, extra.Value.LootId, extra.Value.Amount));
            }
            return items;
        }
    }
}
