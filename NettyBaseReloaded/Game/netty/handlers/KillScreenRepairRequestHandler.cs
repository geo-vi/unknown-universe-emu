using System;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.settings;
using NettyBaseReloaded.Utils;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class KillScreenRepairRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient)
            {
                Console.WriteLine("TODO: Update KillscreenRepairRequestHandler for new client");
                //new NotImplementedException();
            }
            else
            {
                if (gameSession?.Client == null) return;
                var player = gameSession.Player;

                var cmd = new commands.old_client.requests.KillScreenRepairRequest();
                cmd.readCommand(buffer);

                short repairTypeValue = cmd.selection.repairTypeValue;

                var killscreen = Killscreen.Load(player);
                if (killscreen == null) throw new ArgumentNullException(); // handle null

                switch (repairTypeValue)
                {
                    case KillScreenOptionTypeModule.BASIC_REPAIR:
                        killscreen.SelectedOption = 1;
                        break;
                    case KillScreenOptionTypeModule.AT_JUMPGATE_REPAIR:
                        killscreen.SelectedOption = 2;
                        break;
                    case KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR:
                        killscreen.SelectedOption = 3;
                        break;
                }

                var price = killscreen.Price;
                switch (price.Item1)
                {
                    case PriceModule.CREDITS:
                        if (player.Information.Credits.Get() >= price.Item2)
                        {
                            player.Information.Credits.Remove(price.Item2);
                            player.Controller.Destruction.RespawnPlayer();
                            break;
                        }
                        SendRepairImpossible(gameSession, PriceModule.CREDITS);
                        break;
                    case PriceModule.URIDIUM:
                        if (player.Information.Uridium.Get() >= price.Item2)
                        {
                            player.Information.Uridium.Remove(price.Item2);                           
                            player.Controller.Destruction.RespawnPlayer();
                            break;
                        }
                        SendRepairImpossible(gameSession, PriceModule.URIDIUM);
                        break;
                }
                player.Save();
            }
        }

        private async void PrepareRespawn()
        {
            await Task.Delay(1000);
        }

        public void SendRepairImpossible(GameSession gameSession, short currency)
        {
            if (gameSession.Player.UsingNewClient)
            {
                new NotImplementedException();
            }
            else
            {
                var player = gameSession.Player;
                var killscreen = Killscreen.Load(player);
                var price = killscreen.Price;
                var options = new List<netty.commands.old_client.KillScreenOptionModule>();
                var optionModule = new netty.commands.old_client.KillScreenOptionModule(
                    new netty.commands.old_client.KillScreenOptionTypeModule(netty.commands.old_client.KillScreenOptionTypeModule.BASIC_REPAIR),
                    new netty.commands.old_client.PriceModule(price.Item1, price.Item2),
                    true,
                    0,
                    new netty.commands.old_client.MessageLocalizedWildcardCommand(currency == PriceModule.URIDIUM ? "desc_killscreen_repair_impossible" : "desc_killscreen_repair_credits_impossible", new List<netty.commands.old_client.MessageWildcardReplacementModule>()),
                    new netty.commands.old_client.MessageLocalizedWildcardCommand("", new List<netty.commands.old_client.MessageWildcardReplacementModule>()),
                    new netty.commands.old_client.MessageLocalizedWildcardCommand("ttip_killscreen_free_phoenix", new List<netty.commands.old_client.MessageWildcardReplacementModule>()),
                    new netty.commands.old_client.MessageLocalizedWildcardCommand(currency == PriceModule.URIDIUM ? "btn_killscreen_payment" : "btn_killscreen_get_phoenix", new List<netty.commands.old_client.MessageWildcardReplacementModule>()));
                options.Add(optionModule);

                Packet.Builder.KillScreenUpdateCommand(gameSession, options);
            }
        }
    }
}