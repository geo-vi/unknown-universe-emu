using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class NanoClusterCooldown : Cooldown
    {
        public NanoClusterCooldown(Player player) : base(DateTime.Now, DateTime.Now.AddMinutes(15))
        {
            Send(player.GetGameSession());
        }

        public override void OnFinish(Character character)
        {
        }

        public override void Send(GameSession gameSession)
        {
            Packet.Builder.LegacyModule(gameSession, "0|A|CLD|IH|" + TimeLeft.Seconds, true);
        }
    }
}
