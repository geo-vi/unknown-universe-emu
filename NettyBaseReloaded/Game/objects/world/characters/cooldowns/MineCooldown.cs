using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class MineCooldown : Cooldown
    {
        public MineCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(20))
        {
        }

        public override void OnFinish(Character character)
        {
            
        }

        public override void Send(GameSession gameSession)
        {
            Packet.Builder.LegacyModule(gameSession, "0|A|CLD|MIN|" + TimeLeft.Seconds, true);

        }
    }
}
