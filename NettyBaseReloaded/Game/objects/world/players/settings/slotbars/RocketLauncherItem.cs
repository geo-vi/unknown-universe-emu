using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class RocketLauncherItem : SlotbarItem
    {
        public RocketLauncherItem(string itemId, int counterValue, int maxCounter,
            List<ClientUITooltipTextFormat> tooltips = null, short counterType = 2,
            bool selected = false, bool visible = true, bool buyable = false)
            : base(itemId, counterValue, maxCounter, tooltips, counterType, selected, visible, buyable)
        {
            if (itemId.Equals("equipment_weapon_rocketlauncher_not_present"))
            {
                CounterValue = 0;
                Activable = false;

                CounterType = SlotbarCategoryItemModule.DOTS;

                ToolTipItemBar = new List<ClientUITooltipTextFormat>
                {
                    new ClientUITooltipTextFormat(ClientUITooltipTextFormat.RED, "ttip_rocketlauncher_unloaded",
                        new commandWw(5), new List<commandF5>())
                };
            }

            if (itemId.Equals("equipment_weapon_rocketlauncher_hst-2") ||
                itemId.Equals("equipment_weapon_rocketlauncher_hst-1"))
            {
                CounterValue = 0;
                MaxCounter = 5;

                CounterType = SlotbarCategoryItemModule.DOTS;

                ToolTipItemBar = new List<ClientUITooltipTextFormat>
                {
                    new ClientUITooltipTextFormat(ClientUITooltipTextFormat.STANDARD, "ttip_rocketlauncher",
                        new commandWw(5),
                        new List<commandF5> {new commandF5("%TYPE%", itemId, new commandWw(1))})
                };
            }

        }

        public override void Execute(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
