using System;
using System.Linq;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class RocketCooldown : Cooldown
    {
        private double CooldownTime;

        internal RocketCooldown(double seconds = 2) : base(DateTime.Now, DateTime.Now.AddSeconds(seconds))
        {
            CooldownTime = seconds;
        }

        public override void OnStart(Character character)
        {
            base.OnStart(character);
        }

        public override void OnFinish(Character character)
        {
            var player = character as Player;
            if (player == null) return;
            if (player.Controller.Attack.Attacking && player.Controller.CPUs.Active.Any(x => x == CPU.Types.AUTO_ROK))
            {
                player.Controller?.Attack.LaunchMissle(player.Settings.CurrentRocket.LootId);
            }
        }

        public override void Send(GameSession gameSession)
        {
            var player = gameSession.Player;

            var item = gameSession.Player.Settings.Slotbar._items[player.Settings.CurrentRocket.LootId];
            if (player.UsingNewClient)
            {
                gameSession.Client.Send(SetCooldown(item.ItemId, TimerState.COOLDOWN, (EndTime - DateTime.Now).Milliseconds, CooldownTime * 100, true));
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession, "0|A|CLD|ROK|"+ TimeLeft.Seconds, true);
            }
        }
    }
}