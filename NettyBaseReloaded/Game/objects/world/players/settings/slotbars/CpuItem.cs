using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.netty.commands.new_client;
using Console = System.Console;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class CpuItem : SlotbarItem
    {
        public CpuItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 0, bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, toolTipItemBar, counterType, selected, visible, buyable)
        {
        }

        public override void Execute(Player player)
        {
            var cpu = player.Extras.FirstOrDefault(x => x.Value.EquipmentItem.Item.LootId == ItemId).Value;
            cpu?.execute();
        }
    }
}