using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
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
            var gameSession = World.StorageManager.GameSessions[player.Id];

            Cooldown cooldown;

            switch (ItemId)
            {
                case "equipment_extra_cpu_ish-01":
                    if (player.Cooldowns.Exists(x => x is ISHCooldown)) return;

                    GameClient.SendRangePacket(player, netty.commands.old_client.LegacyModule.write("0|n|ISH|" + player.Id), true);
                    GameClient.SendRangePacket(player, netty.commands.new_client.LegacyModule.write("0|n|ISH|" + player.Id), true);
                    player.Controller.Effects.SetInvincible(3000);

                    cooldown = new ISHCooldown();
                    player.Cooldowns.Add(cooldown);
                    cooldown.Send(gameSession);
                    break;

                case "ammunition_mine_smb-01":
                    if (player.Cooldowns.Exists(x => x is SMBCooldown)) return;

                    GameClient.SendRangePacket(player, netty.commands.old_client.LegacyModule.write("0|n|SMB|" + player.Id), true);
                    GameClient.SendRangePacket(player, netty.commands.new_client.LegacyModule.write("0|n|SMB|" + player.Id), true);
                    player.Controller.Damage.Area(20, 700, DamageType.PERCENTAGE);

                    cooldown = new SMBCooldown();
                    player.Cooldowns.Add(cooldown);
                    cooldown.Send(gameSession);
                    break;

                case "ammunition_specialammo_emp-01":
                    if (player.Cooldowns.Exists(x => x is EMPCooldown)) return;

                    GameClient.SendRangePacket(player, netty.commands.old_client.LegacyModule.write("0|n|EMP|" + player.Id), true);
                    GameClient.SendRangePacket(player, netty.commands.new_client.LegacyModule.write("0|n|EMP|" + player.Id), true);

                    GameClient.SendRangePacket(player, netty.commands.old_client.LegacyModule.write("0|UI|MM|NOISE|1|1"));
                    GameClient.SendRangePacket(player, netty.commands.new_client.LegacyModule.write("0|UI|MM|NOISE|1|1"));

                    //I can't use GameHandler.SendSelectedPacket because i need to set Selected to null
                    foreach (var entry in player.Spacemap.Entities)
                    {
                        var entity = entry.Value;

                        if (entity.Selected != null && entity.Selected.Id == player.Id && entity is Player)
                        {
                            var entitySession = World.StorageManager.GetGameSession(entity.Id);
                            Packet.Builder.LegacyModule(entitySession, "0|A|STM|msg_own_targeting_harmed");
                            Packet.Builder.ShipSelectionCommand(entitySession, null);
                            entity.Selected = null;
                        }
                    }

                    player.AttachedNpcs.Clear();
                    player.Controller.Effects.NotTargetable(3000);

                    cooldown = new EMPCooldown();
                    player.Cooldowns.Add(cooldown);
                    cooldown.Send(gameSession);
                    break;
            }
        }
    }
}
