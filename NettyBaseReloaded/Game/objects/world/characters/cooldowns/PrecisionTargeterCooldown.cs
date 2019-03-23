using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class PrecisionTargeterCooldown : Cooldown
    {
        public PrecisionTargeterCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(300))
        {
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
            Packet.Builder.LegacyModule(gameSession, "0|A|CLD|RPM|" + TimeLeft.Seconds, true);
            //TODO: do for new client too
        }
    }
}
