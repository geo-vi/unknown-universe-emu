using System;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class ISHCooldown : Cooldown
    {
        internal ISHCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(30)) { }

        public override void OnStart(Character character)
        {
            base.OnStart(character);
        }

        public override void OnFinish(Character character)
        {

        }

        public override void Send(GameSession gameSession)
        {
            if (gameSession.Player.UsingNewClient)
            {
                gameSession.Client.Send(SetCooldown("equipment_extra_cpu_ish-01", TimerState.COOLDOWN, 30000, 30000,true));
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession, "0|A|CLD|ISH|" + TimeLeft.Seconds, true);
            }
        }
    }
}