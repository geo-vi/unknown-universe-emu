using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.techs
{
    class EnergyLeech : Tech
    {
        EnergyLeechCooldown cld = new EnergyLeechCooldown();

        public EnergyLeech(Player player) : base(player)
        {
        }

        public override void Tick()
        {
            if (Active)
            {
                if (TimeFinish < DateTime.Now)
                    Disable();
            }
        }

        public override void execute()
        {
            if (Active || Player.Cooldowns.CooldownDictionary.Any(c => c.Value is EnergyLeechCooldown)) return;
            Active = true;
            Player.Storage.EnergyLeechActivated = true;
            Packet.Builder.TechStatusCommand(Player.GetGameSession());
            GameClient.SendToPlayerView(Player, netty.commands.old_client.LegacyModule.write("0|TX|A|S|ELA|" + Player.Id), true);
            GameClient.SendToPlayerView(Player, netty.commands.new_client.LegacyModule.write("0|TX|A|S|ELA|" + Player.Id), true);
            TimeFinish = DateTime.Now.AddSeconds(900);
            Player.Cooldowns.Add(cld);
        }

        public void ExecuteHeal(int damage)
        {
            if (TimeFinish > DateTime.Now) {
                var lastDamage = (damage / 100) * 10;
                Player.Controller.Heal.Execute(lastDamage);
            }
        }

        private void Disable()
        {
            Active = false;
            Player.Storage.EnergyLeechActivated = false;
            Packet.Builder.TechStatusCommand(Player.GetGameSession());
            GameClient.SendToPlayerView(Player, netty.commands.old_client.LegacyModule.write("0|TX|D|S|ELA|" + Player.Id), true);
            GameClient.SendToPlayerView(Player, netty.commands.new_client.LegacyModule.write("0|TX|D|S|ELA|" + Player.Id), true);
            cld.Send(Player.GetGameSession());
        }
    }
}
