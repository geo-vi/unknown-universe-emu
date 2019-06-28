using System;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;
using Server.Game.netty.commands.new_client;
using Server.Game.netty.commands.new_client.requests;

namespace Server.Game.netty.handlers
{
    class command42JHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            const string STANDARD = "standardSlotBar";
            const string PREMIUM = "premiumSlotBar";
            var cmd = new commandHF();
            cmd.readCommand(buffer);

            Console.WriteLine($"{cmd.targetSlotId} targetSlotId, {cmd.originSlotId} originSlotId, {cmd.targetSlotbar} targetSlotbar, {cmd.originSlotbar} originSlotbar, {cmd.itemId} itemId");

            var standard = gameSession.Player.Settings.Slotbar.QuickslotItems;
            var premium = gameSession.Player.Settings.Slotbar.PremiumQuickslotItems;

            SlotbarQuickslotItem swapItem = null;

            try
            {
                if (cmd.originSlotbar == "")
                {
                    if (cmd.targetSlotbar == STANDARD)
                    {
                        var swapableItems = standard.Where(x => x.slotId == cmd.targetSlotId);
                        if (swapableItems.Any())
                        {
                            swapItem = swapableItems.FirstOrDefault();
                            standard.Remove(swapItem);
                        }
                        standard.Add(new SlotbarQuickslotItem(cmd.targetSlotId, cmd.itemId));
                    }
                    else
                    {
                        var swapableItems = premium.Where(x => x.slotId == cmd.targetSlotId);
                        if (swapableItems.Any())
                        {
                            swapItem = swapableItems.FirstOrDefault();
                            standard.Remove(swapItem);
                        }
                        premium.Add(new SlotbarQuickslotItem(cmd.targetSlotId, cmd.itemId));
                    }
                }
                else if (cmd.originSlotbar == STANDARD)
                {
                    if (cmd.targetSlotbar == "")
                    {
                        standard.Remove(standard.FirstOrDefault(x => x.slotId == cmd.originSlotId));
                    }
                    else if (cmd.targetSlotbar == STANDARD)
                    {
                        var swapableItems = standard.Where(x => x.slotId == cmd.targetSlotId);
                        if (swapableItems.Any())
                        {
                            standard.FirstOrDefault(x => x.slotId == cmd.originSlotId).slotId = cmd.targetSlotId;
                            standard.FirstOrDefault(x => x.slotId == cmd.targetSlotId && x.lootId != cmd.itemId)
                                    .slotId =
                                cmd.originSlotId;
                        }
                        else
                        {
                            standard.FirstOrDefault(x => x.slotId == cmd.originSlotId).slotId = cmd.targetSlotId;
                        }
                    }
                    else
                    {
                        var swapableItems = premium.Where(x => x.slotId == cmd.targetSlotId);
                        if (swapableItems.Any())
                        {
                            standard.FirstOrDefault(x => x.slotId == cmd.originSlotId).lootId =
                                premium.FirstOrDefault(x => x.slotId == cmd.targetSlotId).lootId;
                            premium.FirstOrDefault(x => x.slotId == cmd.targetSlotId && x.lootId != cmd.itemId).lootId =
                                cmd.itemId;
                        }
                        else
                        {
                            standard.Remove(standard.FirstOrDefault(x => x.slotId == cmd.originSlotId));
                            premium.Add(new SlotbarQuickslotItem(cmd.targetSlotId, cmd.itemId));
                        }
                    }
                }
                else if (cmd.originSlotbar == PREMIUM)
                {
                    if (cmd.targetSlotbar == "")
                    {
                        premium.Remove(premium.FirstOrDefault(x => x.slotId == cmd.originSlotId));
                    }
                    else if (cmd.targetSlotbar == PREMIUM)
                    {
                        var swapableItems = premium.Where(x => x.slotId == cmd.targetSlotId);
                        if (swapableItems.Any())
                        {
                            premium.FirstOrDefault(x => x.slotId == cmd.originSlotId).slotId = cmd.targetSlotId;
                            premium.FirstOrDefault(x => x.slotId == cmd.targetSlotId && x.lootId != cmd.itemId).slotId =
                                cmd.originSlotId;
                        }
                        else
                        {
                            premium.FirstOrDefault(x => x.slotId == cmd.originSlotId).slotId = cmd.targetSlotId;
                        }
                    }
                    else
                    {
                        var swapableItems = standard.Where(x => x.slotId == cmd.targetSlotId);
                        if (swapableItems.Any())
                        {
                            premium.FirstOrDefault(x => x.slotId == cmd.originSlotId).lootId =
                                standard.FirstOrDefault(x => x.slotId == cmd.targetSlotId).lootId;
                            standard.FirstOrDefault(x => x.slotId == cmd.targetSlotId && x.lootId != cmd.itemId)
                                    .lootId =
                                cmd.itemId;
                        }
                        else
                        {
                            premium.Remove(premium.FirstOrDefault(x => x.slotId == cmd.originSlotId));
                            standard.Add(new SlotbarQuickslotItem(cmd.targetSlotId, cmd.itemId));
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }

            Packet.Builder.SendSlotbars(gameSession);
        }
    }
}
