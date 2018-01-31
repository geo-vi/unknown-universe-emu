using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.extra.techs;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class TechItem : SlotbarItem
    {
        public TechItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 1, bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, toolTipItemBar, counterType, selected, visible, buyable)
        {
            CounterType = SlotbarCategoryItemModule.NONE;
        }

        public override void Execute(Player player)
        {
            var gameSession = World.StorageManager.GameSessions[player.Id];

            foreach (var tech in gameSession.Player.Techs)
            {
                switch (ItemId)
                {
                    case "tech_battle-repair-bot":
                        if (tech is BattleRepairRobot)
                        {
                            tech.execute();
                        }
                        break;
                    case "tech_backup-shields":
                        if (tech is ShieldBuff)
                        {
                            tech.execute();
                        }
                        break;
                    case "tech_precision-targeter":
                        if (tech is RocketPrecission)
                        {
                            tech.execute();
                        }
                        break;
                    case "tech_energy-leech":
                        if (tech is EnergyLeech)
                        {
                            tech.execute();
                        }
                        break;
                    case "tech_chain-impulse":
                        if (tech is ChainImpulse)
                        {
                            tech.execute();
                        }
                        break;
                }
            }
        }
    }
}