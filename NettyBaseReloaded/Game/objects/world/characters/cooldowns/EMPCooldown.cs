using System;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class EMPCooldown : Cooldown
    {
        internal EMPCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(30)) { }

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
                gameSession.Client.Send(SetCooldown("ammunition_specialammo_emp-01", TimerState.COOLDOWN, TimeLeft.TotalMilliseconds, TotalTime.TotalMilliseconds,true));
            }
            else
            {
                Packet.Builder.LegacyModule(gameSession, "0|A|CLD|EMP|" + TimeLeft.Seconds, true);
            }
        }
    }
}