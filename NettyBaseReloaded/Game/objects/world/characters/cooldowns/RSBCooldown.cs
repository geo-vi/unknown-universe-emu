using System;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class RSBCooldown : Cooldown
    {
        internal RSBCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(3)) { }

        public override void OnStart(Character character)
        {
        }

        public override void OnFinish(Character character)
        {
        }

        public override void Send(GameSession gameSession)
        {
            var player = gameSession.Player;

            if (player.UsingNewClient)
            {
                gameSession.Client.Send(SetCooldown("ammunition_laser_rsb-75", TimerState.COOLDOWN, TimeLeft.TotalMilliseconds, TotalTime.TotalMilliseconds, true));
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession, "0|A|CLD|RSB|" + TimeLeft.Seconds, true);
            }
        }
    }
}