using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.techs
{
    class ShieldBuff : Tech
    {
        public ShieldBuff(Player player) : base(player)
        {
        }

        public override void Tick()
        {
        }

        public override void execute()
        {
            if (Player.Cooldowns.CooldownDictionary.Any(c => c.Value is ShieldBuffCooldown)) return;
            GameClient.SendToPlayerView(Player, netty.commands.old_client.LegacyModule.write("0|TX|A|S|SBU|" + Player.Id), true);
            GameClient.SendToPlayerView(Player, netty.commands.new_client.LegacyModule.write("0|TX|A|S|SBU|" + Player.Id), true);
            ExecuteShield();
            Disable();
        }

        private void ExecuteShield()
        {
            Player.Controller.Heal.Execute(75000, 0, HealType.SHIELD);
        }

        private void Disable()
        {
            var cld = new ShieldBuffCooldown();
            cld.Send(Player.GetGameSession());
            Player.Cooldowns.Add(cld);
        }
    }
}
