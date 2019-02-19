using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class SpecialItem : SlotbarItem
    {
        public SpecialItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 2, bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, toolTipItemBar, counterType, selected, visible, buyable)
        {
        }

        public override void Execute(Player player)
        {
            switch (ItemId)
            {
                case "equipment_extra_cpu_ish-01":
                    player.Controller.Specials.InstantShield();
                    break;
                case "ammunition_mine_smb-01":
                    player.Controller.Specials.Smartbomb();
                    break;
                case "ammunition_specialammo_emp-01":
                    player.Controller.Specials.EMP();
                    break;
                case "ammunition_firework_fwx-s":
                    player.Controller.Specials.PlaceFirework(1);
                    break;
                case "ammunition_firework_fwx-m":
                    player.Controller.Specials.PlaceFirework(2);
                    break;
                case "ammunition_firework_fwx-l":
                    player.Controller.Specials.PlaceFirework(3);
                    break;
                case "ammunition_firework_ignite":
                    player.Controller.Specials.IgniteFireworks();
                    break;
            }
        }
    }
}
