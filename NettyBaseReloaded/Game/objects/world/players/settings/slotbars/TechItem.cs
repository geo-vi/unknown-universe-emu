using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class TechItem : SlotbarItem
    {
        public TechItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 1, bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, toolTipItemBar, counterType, selected, visible, buyable)
        {
            CounterType = SlotbarCategoryItemModule.NUMBER;
        }

        public override void Execute(Player player)
        {
            throw new NotImplementedException();
        }
    }
}