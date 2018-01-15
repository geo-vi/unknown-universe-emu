using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.players.extra.techs
{
    class RocketPrecission : Tech
    {
        public RocketPrecission(Player player) : base(player)
        {
        }

        public override void Tick()
        {
            if (Active)
            {
                if (TimeFinish < DateTime.Now) End();
            }
        }

        public override void execute()
        {
            //if (Player.Cooldowns.Exists(x => x is RocketPrecissionCooldown)) return;
            //var cld = new RocketPrecissionCooldown();
            //cld.Send(Player.GetGameSession());
            //Player.Cooldowns.Add(cld);

            Active = true;
            TimeFinish = DateTime.Now.AddMinutes(15);
        }

        public void End()
        {
            Active = false;
            
        }
    }
}
