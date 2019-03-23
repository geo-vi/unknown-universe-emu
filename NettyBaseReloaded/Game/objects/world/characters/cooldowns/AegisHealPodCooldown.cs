using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.extra.abilities;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class AegisHealPodCooldown : Cooldown
    {
        public AegisHealPodCooldown(AegisHealPod ability) : base(DateTime.Now, DateTime.Now.AddSeconds(120))
        {
            Send(ability.Player.GetGameSession());
        }

        public override void OnStart(Character character)
        {
            base.OnStart(character);
        }

        public override void OnFinish(Character character)
        {
        }

        public override void Send(GameSession gameSession)
        {
            Packet.Builder.LegacyModule(gameSession, "0|A|CLD|HPD|" + TimeLeft.Seconds, true);
        }
    }
}
