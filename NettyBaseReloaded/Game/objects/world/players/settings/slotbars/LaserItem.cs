using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class LaserItem : SlotbarItem
    {
        public LaserItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> tooltips = null, short counterType = 2,
            bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, tooltips, counterType, selected, visible, buyable) { }

        public override void Execute(Player player)
        {
            var gameSession = World.StorageManager.GameSessions[player.Id];

            foreach (var item in player.Settings.Slotbar._items)
            {
                var value = item.Value;

                if (value is LaserItem)
                {
                    value.Selected = false;
                    if (gameSession.Player.UsingNewClient)
                        gameSession.Client.Send(value.ChangeStatus()); //TODO fix this
                }
            }

            player.Settings.CurrentAmmo = player.Information.Ammunitions[ItemId];
            Selected = true;

            if (player.Controller.Attack.Attacking && ItemId.Equals("ammunition_laser_rsb-75"))
            {
                player.Controller.Attack.LaserAttack();
            }

            if (gameSession.Player.UsingNewClient)
                gameSession.Client.Send(ChangeStatus()); //TODO Same as above
        }
    }
}
