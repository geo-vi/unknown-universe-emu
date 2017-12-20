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

        protected Extra(Player player, int itemId, string lootId, int amount) : base(itemId, lootId, amount)
        {
            Player = player;
        }

        public abstract void execute();

        public static Dictionary<string, Extra> LoadExtras(Player player, Dictionary<string, Item> consumables)
        {
            var extras = new Dictionary<string, Extra>();
            foreach (var consumable in consumables)
            {
                switch (consumable.Key)
                {
                    case "equipment_extra_cpu_ajp-01":
                        // TODO add Jump
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
                        // TODO: add auto rocket
                        break;
                    case "equipment_extra_cpu_rllb-x":
                        // TODO: add auto rocket launcher
                        break;
                    case "equipment_extra_cpu_dr-01":
                    case "equipment_extra_cpu_dr-02":
                        // TODO: add drone rep
                        break;
                }
            }


            return extras;
        }
    }
}
