using System;
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
        }

        public override void OnFinish(Character character)
        {
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
                Packet.Builder.LegacyModule(gameSession, "0|A|CLD|ROK|"+CooldownTime);
            }
        }
    }
}