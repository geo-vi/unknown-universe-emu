using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class RocketLauncherCooldown : Cooldown
    {
        public RocketLauncherCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(5))
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
        }
    }
}
