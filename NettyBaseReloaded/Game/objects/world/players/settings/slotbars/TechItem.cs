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
            var techs = gameSession.Player.Techs;
            switch (ItemId)
            {
                case "tech_battle-repair-bot":
                    if (techs.ContainsKey(Techs.BATTLE_REPAIR_ROBOT))
                        techs[Techs.BATTLE_REPAIR_ROBOT].execute();
                    break;
                case "tech_backup-shields":
                    if (techs.ContainsKey(Techs.SHIELD_BUFF))
                        techs[Techs.SHIELD_BUFF].execute();
                    break;
                case "tech_precision-targeter":
                    if (techs.ContainsKey(Techs.ROCKET_PRECISSION))
                        techs[Techs.ROCKET_PRECISSION].execute();
                    break;
                case "tech_energy-leech":
                    if (techs.ContainsKey(Techs.ENERGY_LEECH))
                        techs[Techs.ENERGY_LEECH].execute();
                    break;
                case "tech_chain-impulse":
                    if (techs.ContainsKey(Techs.CHAIN_IMPULSE))
                        techs[Techs.CHAIN_IMPULSE].execute();
                    break;
            }
        }
    }
}