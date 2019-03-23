using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.techs
{
    class ChainImpulse : Tech
    {
        public ChainImpulse(Player player) : base(player)
        {
        }

        public override void Tick()
        {
        }

        public override void execute()
        {
            if (Player.Cooldowns.CooldownDictionary.Any(c => c.Value is ChainImpulseCooldown) || Player.State.InDemiZone) return;
            Player.Controller.Damage?.ECI();
            Disable();
        }

        private void Disable()
        {
            var cld = new ChainImpulseCooldown();
            cld.Send(Player.GetGameSession());
            Player.Cooldowns.Add(cld);
        }
    }
}
