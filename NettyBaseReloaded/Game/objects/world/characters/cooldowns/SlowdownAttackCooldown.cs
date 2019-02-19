using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class SlowdownAttackCooldown : Cooldown
    {
        public SlowdownAttackCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(10))
        {
        }


        public override void OnFinish(Character character)
        {
        }

        public override void Send(GameSession gameSession)
        {
        }
    }
}
