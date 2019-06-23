using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.quests;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using VisualModifierCommand = NettyBaseReloaded.Game.netty.commands.old_client.VisualModifierCommand;

namespace NettyBaseReloaded.Game.controllers.login
{
    class Regular : ILogin
    {
        public Regular(GameSession gameSession) : base(gameSession)
        {
        }

        public override void Execute()
        {
            //InitiateEvents();
            SendSettings();
            Spawn();
            SendLegacy();
            Console.WriteLine("Successfully logged in: " + GameSession.Player.Id + " Client used: " + (GameSession.Player.UsingNewClient ? "10.1" : "7.5.3") + " [" + GameSession.Player.Name + "]");
        }

        private void Spawn()
        {
            var player = GameSession.Player;
            if (!player.Spacemap.Entities.ContainsKey(player.Id))
            {
                player.Spacemap.AddEntity(player);
            }
            else
            {
                player.Storage.Clean();
            }

            Packet.Builder.ShipInitializationCommand(GameSession);
            Packet.Builder.DronesCommand(GameSession, GameSession.Player);

            if (GameSession.Player.Information.GameBans.Any(x => x.Value.Expiry > DateTime.Now))
            {
                var ban = GameSession.Player.Information.GameBans.FirstOrDefault(x => x.Value.Expiry > DateTime.Now);
                Packet.Builder.LegacyModule(GameSession, "0|A|STD|You've been banned by " + ban.Value.GetBanAccountant().Name + " at " + ban.Value.IssuedTime + "#" + ban.Key + "\n" + ban.Value.Reason);
                GameSession.Kick();
            }
            GameSession.Player.State.StartLoginProtection();
        }
    }
}
