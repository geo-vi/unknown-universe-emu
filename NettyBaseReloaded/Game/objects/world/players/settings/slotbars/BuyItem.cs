using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class BuyItem : SlotbarItem
    {
        public BuyItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 2, bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, toolTipItemBar, counterType, selected, visible, buyable)
        {
            ClickedId = "buy_" + itemId;
            Buyable = true;
            CounterType = SlotbarCategoryItemModule.NUMBER;
        }

        public override void Execute(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
