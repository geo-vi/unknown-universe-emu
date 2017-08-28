using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.settings.new_client_slotbars
{
    class CpuItem : SlotbarItem
    {
        public CpuItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 0, bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, toolTipItemBar, counterType, selected, visible, buyable)
        {
        }

        public override void Execute(Player player)
        {
            switch (ItemId)
            {
                case "equipment_extra_repbot_rep-s":
                case "equipment_extra_repbot_rep-1":
                case "equipment_extra_repbot_rep-2":
                case "equipment_extra_repbot_rep-3":
                case "equipment_extra_repbot_rep-4":
                    Console.WriteLine("AD");
                    player.Controller.Repairing = true;
                    break;
            }
        }
    }
}