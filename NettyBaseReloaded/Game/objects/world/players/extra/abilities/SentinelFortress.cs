using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class SentinelFortress : Ability
    {
        public SentinelFortress(Player player) : base(player, Abilities.SHIP_ABILITY_SENTINEL_FORTRESS)
        {
        }

        public override void Tick()
        {
            Update();
        }

        public override void execute()
        {
            if (!Enabled) return;
            Active = true;
            TargetIds.Add(Player.Id);
            TimeFinish = DateTime.Now.AddMinutes(2);
            Start();
        }

        public override void ThreadUpdate()
        {
            if (TimeFinish < DateTime.Now)
            {
                End();
                Active = false;
                Cooldown = new SentinelFortressCooldown(Player);
            }
        }
    }
}
